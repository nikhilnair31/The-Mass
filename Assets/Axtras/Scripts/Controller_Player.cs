using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.Cinemachine;
using DG.Tweening;

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

    [Header("Zoom Settings")]
    [SerializeField] private Volume volume;
    [SerializeField] private Vignette vignette;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private float zoomFOV = 40f;
    private float currFOV;
    private bool isZoomed = false;
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
        
        if (volume.profile.TryGet(out Vignette v)) {
            vignette = v;
        }
        currFOV = cam.Lens.FieldOfView;
    }

    private void Update() {
        HandleMouseLook();
        HandleMovement();
        HandleInteractable();
        HandleZoom();
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
                else if (currentInteractable.TryGetComponent(out Interactable_Drawer drawer)) {
                    drawer.ControlOpenCloseDrawer();
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
            if (heldInteractable.TryGetComponent(out Interactable_Throwable throwable)) {
                throwLineGO.SetActive(true);
            }
            else if (heldInteractable.TryGetComponent(out Interactable_Pokable pokable)) {
                
            }
            else if (heldInteractable.TryGetComponent(out Interactable_Spray spray)) {
                spray.StartSpray();
            }
        }
        if (Input.GetMouseButtonUp(0) && heldInteractable != null) {
            if (heldInteractable.TryGetComponent(out Interactable_Throwable throwable)) {
                throwable.ThrowInteractable();
                throwLineGO.SetActive(false);
            }
            else if (heldInteractable.TryGetComponent(out Interactable_Pokable pokable)) {
                pokable.PokableInteractable();
            }
            else if (heldInteractable.TryGetComponent(out Interactable_Spray spray)) {
                spray.StopSpray();
            }
        }
    }
    private void HandleZoom() {
        if (Input.GetMouseButtonDown(1)) {
            isZoomed = true;

            DOTween.To(
                () => cam.Lens.FieldOfView,
                x => {
                    var lens = cam.Lens;
                    lens.FieldOfView = x;
                    cam.Lens = lens;
                },
                zoomFOV,
                0.5f
            );

            DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0.5f, 0.5f);
        }
        else if (Input.GetMouseButtonUp(1)) {
            isZoomed = false;

            DOTween.To(
                () => cam.Lens.FieldOfView,
                x => {
                    var lens = cam.Lens;
                    lens.FieldOfView = x;
                    cam.Lens = lens;
                },
                currFOV,
                0.5f
            );

            DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0f, 0.5f);
        }
    }

    private void CheckForInteractable() {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, maxDistance, interactableLayer)) {
            if (hit.transform != currentInteractable) {
                if (hit.transform.TryGetComponent(out Controller_Interactables interactable)) {
                    currentInteractable = hit.transform;
                    var showTextStr = interactable.ReturnInteractableText();
                    Manager_Thoughts.Instance.UpdateThoughtText(showTextStr);
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
