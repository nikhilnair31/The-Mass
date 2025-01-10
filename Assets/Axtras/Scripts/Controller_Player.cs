using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    #region Vars
    public static Controller_Player Instance { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float normalHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private Transform playerTransform;
    private Rigidbody rb;
    private Vector3 moveDirection;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform playerCamera;
    private float xRotation = 0f;

    [Header("Interactable Settings")]
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] public Transform holdAtTransform;
    [SerializeField] private GameObject throwLineGO;
    [SerializeField] public Transform heldInteractable;
    private Transform currentInteractable;
    private RaycastHit hit;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update() {
        HandleMouseLook();
        HandleMovement();
        HandleInteractable();
        CheckForInteractable();
    }

    private void HandleMouseLook() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    private void HandleMovement() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDirection = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftControl)) {
            rb.linearVelocity = new Vector3(moveDirection.x * crouchSpeed, rb.linearVelocity.y, moveDirection.z * crouchSpeed);
            playerTransform.localScale = new Vector3(1f, crouchHeight / normalHeight, 1f); // Adjust height
        } 
        else {
            rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
            playerTransform.localScale = new Vector3(1f, 1f, 1f); // Reset height
        }

        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl)) {
            rb.linearVelocity = new Vector3(moveDirection.x * sprintSpeed, rb.linearVelocity.y, moveDirection.z * sprintSpeed);
        }

        rb.AddForce(Vector3.up * gravity);
    }
    private void HandleInteractable() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (currentInteractable != null) {
                if (currentInteractable.TryGetComponent(out Controller_Pickable pickable)) {
                    if (pickable.ReturnPickableBool()) {
                        pickable.PickInteractable();
                        pickable.SetWasPicked(true);
                    }
                }
                
                else if (currentInteractable.TryGetComponent(out Interactable_Blinds blinds)) {
                    blinds.OpenCloseBlinds();
                }
                else if (currentInteractable.TryGetComponent(out Interactable_Door doors)) {
                    doors.ControlOpenCloseDoor();
                }
                else if (currentInteractable.TryGetComponent(out Interactable_Switch switches)) {
                    switches.ControlOnOffLight();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && heldInteractable != null) {
            if (heldInteractable.TryGetComponent(out Controller_Pickable pickable)) {
                pickable.DropInteractable();
                pickable.SetWasPicked(false);
            }
        }
        
        if (Input.GetMouseButtonDown(0) && heldInteractable != null) {
            if (heldInteractable.TryGetComponent(out Controller_Pickable pickable)) {
                throwLineGO.SetActive(true);
            }
        }
        if (Input.GetMouseButtonUp(0) && heldInteractable != null) {
            if (heldInteractable.TryGetComponent(out Interactable_Throwable throwable)) {
                throwable.ThrowInteractable();
            }
            else if (heldInteractable.TryGetComponent(out Interactable_Pokable pokable)) {
                pokable.PokableInteractable();
            }
            throwLineGO.SetActive(false);
        }
    }

    private void CheckForInteractable() {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, maxDistance, interactableLayer)) {
            if (hit.transform != currentInteractable) {
                if (hit.transform.TryGetComponent(out Controller_Interactables interactable)) {
                    currentInteractable = hit.transform;
                    Manager_Thoughts.Instance.UpdateThoughtText(interactable);
                }
            }
        }
        else {
            if (currentInteractable != null) {
                currentInteractable = null;
                Manager_Thoughts.Instance.ClearThoughtText();
            }
        }
    }

    private void OnDrawGizmos() {
        // Only draw the Gizmos if the ray has hit something
        if (hit.collider != null) {
            // Draw the ray as a line
            Gizmos.color = Color.red;
            Gizmos.DrawLine(hit.point - hit.normal * 0.5f, hit.point);

            // Draw a sphere at the hit point
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hit.point, 0.1f);
        }
    }
}
