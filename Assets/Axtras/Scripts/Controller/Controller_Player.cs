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
    [SerializeField] private float crouchHeight = 1f;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private float camLocalY;

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
    private Transform lookingAtInteractable;
    private RaycastHit hit;

    [Header("Zoom Settings")]
    [SerializeField] private CinemachineVolumeSettings volume;
    [SerializeField] private Vignette vignette;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private float zoomFOV = 40f;
    private float currFOV;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float footstepInterval = 0.5f;
    private float footstepTimer = 0f;
    private bool isMoving = false;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        if (transform.TryGetComponent(out Rigidbody rgb)) {
            rb = rgb;
        }
        if (volume.Profile.TryGet(out Vignette v)) {
            vignette = v;
        }

        rb.freezeRotation = true;
        currFOV = cam.Lens.FieldOfView;
        camLocalY = playerCamera.transform.position.y;
    }

    private void Update() {
        HandleMouseLook();
        HandleMovement();
        HandleInteractable();
        HandleZoom();
        PlayFootsteps();
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

        isMoving = moveDirection.magnitude > 0;

        if (Input.GetKey(KeyCode.LeftControl)) {
            rb.linearVelocity = new Vector3(moveDirection.x * crouchSpeed, rb.linearVelocity.y, moveDirection.z * crouchSpeed);
            playerCamera.DOMoveY(camLocalY - crouchHeight, 0.2f);
        } 
        else {
            rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
            playerCamera.DOMoveY(camLocalY, 0.2f);
        }

        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl)) {
            rb.linearVelocity = new Vector3(moveDirection.x * sprintSpeed, rb.linearVelocity.y, moveDirection.z * sprintSpeed);
        }

        rb.AddForce(Vector3.up * gravity);
    }
    private void HandleInteractable() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (lookingAtInteractable != null) {
                if (lookingAtInteractable.TryGetComponent(out Controller_Pickable pickable)) {
                    if (pickable.ReturnPickableBool() && !heldInteractable) {
                        pickable.PickInteractable();
                        pickable.SetWasPicked(true);
                    }
                }
                
                else if (lookingAtInteractable.TryGetComponent(out Interactable_Blinds blinds)) {
                    blinds.OpenCloseBlinds();
                }
                else if (lookingAtInteractable.TryGetComponent(out Interactable_Door doors)) {
                    doors.ControlOpenCloseDoor();
                }
                else if (lookingAtInteractable.TryGetComponent(out Interactable_Drawer drawer)) {
                    drawer.ControlOpenCloseDrawer();
                }
                else if (lookingAtInteractable.TryGetComponent(out Interactable_Switch switches)) {
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

    private void PlayFootsteps() {
        if (isMoving && rb.linearVelocity.magnitude > 0.1f) {
            footstepTimer += Time.deltaTime;

            if (footstepTimer >= footstepInterval) {
                Helper.Instance.PlayRandAudio(audioSource, footstepClips);
                footstepTimer = 0f;
            }
        }
        else {
            footstepTimer = 0f;
        }
    }

    private void CheckForInteractable() {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, maxDistance, interactableLayer)) {
            // Debug.Log($"hit.transform: {hit.transform} | lookingAtInteractable.name: {lookingAtInteractable.name}");
            
            var interactable = hit.transform.GetComponent<Controller_Interactables>();
            
            // Update if we hit a new interactable object
            if (interactable != null && hit.transform != lookingAtInteractable) {
                lookingAtInteractable = hit.transform;
                var showTextStr = interactable.ReturnInteractableText();
                Manager_Thoughts.Instance.UpdateThoughtText(showTextStr);
                return;
            }
            // Skip if we hit the same interactable object
            else if (interactable != null && hit.transform == lookingAtInteractable) {
                return;
            }
            // Clear if we hit non-interactable object
            else if (interactable == null) {
                lookingAtInteractable = null;
                Manager_Thoughts.Instance.ClearThoughtText();
            }
        }
        else {
            // If we didn't hit anything in the interactable layer
            if (lookingAtInteractable != null) {
                lookingAtInteractable = null;
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
