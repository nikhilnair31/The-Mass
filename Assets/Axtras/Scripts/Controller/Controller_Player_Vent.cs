using UnityEngine;

public class Controller_Player_Vent : MonoBehaviour 
{
    #region Vars
    public static Controller_Player_Vent Instance { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    private Rigidbody rb;
    private Vector3 previousPosition;
    
    [Header("Look Settings")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float maxLookAngle = 5f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private Quaternion initialRotation;
    private Vector3 initialForward;
    
    [Header("Ray Settings")]
    [SerializeField] private float rayDistance = 1f;
    [SerializeField] private LayerMask ventWallLayer;
    private bool forwardWall;
    private bool leftWall;
    private bool rightWall;
    private bool upWall;
    private bool downWall;
    
    [Header("Torch Settings")]
    [SerializeField] private Interactable_Torch torch;

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
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
        rb.useGravity = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        initialRotation = transform.localRotation;
        initialForward = transform.forward;
    }

    private void Update() {
        CheckSides();
        HandleMouseLook();
        HandleMovement();
        HandleInteractable();
        PlayFootsteps();
    }
    private void HandleMouseLook() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        float newXRotation = xRotation - mouseY;
        float newYRotation = yRotation + mouseX;
        
        Quaternion potentialRotation = initialRotation * Quaternion.Euler(newXRotation, newYRotation, 0f);

        xRotation = newXRotation;
        yRotation = newYRotation;
        transform.localRotation = potentialRotation;
    }
    private void HandleMovement() {
        var vert = Input.GetAxis("Vertical");
        var movement = transform.forward * vert * moveSpeed * Time.deltaTime;
        isMoving = movement.magnitude > 0;
        rb.AddForce(movement, ForceMode.VelocityChange);
    }
    private void HandleInteractable() {
        if (Input.GetMouseButtonDown(0)) {
            if (torch != null) {
                torch.ToggleSwitch();
            }
        }
    }

    private void PlayFootsteps() {
        bool hasMoved = Vector3.Distance(transform.position, previousPosition) > 0.01f;
        if (isMoving && hasMoved && rb.linearVelocity.magnitude > 1f) {
            footstepTimer += Time.deltaTime;

            if (footstepTimer >= footstepInterval) {
                Helper.Instance.PlayRandAudio(audioSource, footstepClips);
                footstepTimer = 0f;
            }
        }
        else {
            footstepTimer = 0f;
        }
        previousPosition = transform.position;
    }

    private void CheckSides() {
        // forwardWall = Physics.Raycast(transform.position, transform.forward, rayDistance, ventWallLayer);
        // leftWall = Physics.Raycast(transform.position, -transform.right, rayDistance, ventWallLayer);
        // rightWall = Physics.Raycast(transform.position, transform.right, rayDistance, ventWallLayer);
        // upWall = Physics.Raycast(transform.position, transform.up, rayDistance, ventWallLayer);
        // downWall = Physics.Raycast(transform.position, -transform.up, rayDistance, ventWallLayer);

        // Debug.DrawRay(transform.position, transform.forward * rayDistance, forwardWall ? Color.red : Color.green);
        // Debug.DrawRay(transform.position, -transform.right * rayDistance, leftWall ? Color.red : Color.green);
        // Debug.DrawRay(transform.position, transform.right * rayDistance, rightWall ? Color.red : Color.green);
        // Debug.DrawRay(transform.position, transform.up * rayDistance, upWall ? Color.red : Color.green);
        // Debug.DrawRay(transform.position, -transform.up * rayDistance, downWall ? Color.red : Color.green);

        forwardWall = true;
        leftWall = true;
        rightWall = true;
        upWall = true;
        downWall = true;
    }
}