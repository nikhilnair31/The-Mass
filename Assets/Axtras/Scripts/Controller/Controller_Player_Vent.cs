using UnityEngine;

public class Controller_Player_Vent : MonoBehaviour 
{
    #region Vars
    public static Controller_Player_Vent Instance { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    private Rigidbody rb;
    
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
    }
    private void HandleMouseLook() {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        // Calculate the potential new rotation
        float newXRotation = xRotation - mouseY;
        float newYRotation = yRotation + mouseX;
        
        Quaternion potentialRotation = initialRotation * Quaternion.Euler(newXRotation, newYRotation, 0f);
        Vector3 potentialForward = potentialRotation * Vector3.forward;

        // Project the potential forward vector onto the horizontal and vertical planes
        Vector3 horizontalForward = new Vector3(potentialForward.x, 0, potentialForward.z).normalized;
        Vector3 verticalForward = new Vector3(0, potentialForward.y, potentialForward.z).normalized;
        
        // Calculate separate angles for horizontal and vertical deviation
        float horizontalAngle = Vector3.Angle(new Vector3(initialForward.x, 0, initialForward.z).normalized, horizontalForward);
        float verticalAngle = Vector3.Angle(new Vector3(0, initialForward.y, initialForward.z).normalized, verticalForward);

        bool allowRotation = true;

        // Check horizontal constraints (left/right walls)
        if (leftWall && horizontalAngle > maxLookAngle && Vector3.Dot(horizontalForward, -transform.right) > 0)
            allowRotation = false;
        if (rightWall && horizontalAngle > maxLookAngle && Vector3.Dot(horizontalForward, transform.right) > 0)
            allowRotation = false;

        // Check vertical constraints (up/down walls)
        float upMaxAngle = upWall ? maxLookAngle : 85f;
        float downMaxAngle = downWall ? maxLookAngle : 85f;

        if (verticalAngle > upMaxAngle && potentialForward.y > initialForward.y)
            allowRotation = false;
        if (verticalAngle > downMaxAngle && potentialForward.y < initialForward.y)
            allowRotation = false;

        // Apply rotation if allowed
        if (allowRotation) {
            xRotation = newXRotation;
            yRotation = newYRotation;
            transform.localRotation = potentialRotation;
        }
    }
    private void HandleMovement() {
        var vert = Input.GetAxis("Vertical");
        var movement = transform.forward * vert * moveSpeed * Time.deltaTime;
        rb.AddForce(movement, ForceMode.VelocityChange);
    }
    private void HandleInteractable() {
        if (Input.GetMouseButtonDown(0)) {
            if (torch != null) {
                torch.ToggleSwitch();
            }
        }
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