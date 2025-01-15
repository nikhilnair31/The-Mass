using UnityEngine;

public class Test_Controller_Vent_Player : MonoBehaviour 
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float turnSpeed = 2f;
    
    [Header("Look Constraints")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float verticalLookLimit = 15f;
    [SerializeField] private float horizontalLookLimit = 30f;
    [SerializeField] private float cornerTurnAngle = 90f;
    
    [Header("Corner Detection")]
    [SerializeField] private float rayDistance = 1f;
    [SerializeField] private LayerMask ventWallLayer;
    [SerializeField] private float cornerCheckSpacing = 0.2f;
    [SerializeField] private bool isAtCorner = false;
    
    private Rigidbody rb;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float baseYRotation = 0f; // Store the base forward direction
    private float baseXRotation = 0f;
    private bool forwardWall;
    private bool leftWall;
    private bool rightWall;
    private bool upWall;
    private bool downWall;
    
    private void Start() {
        rb = GetComponent<Rigidbody>();
        
        // Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Store initial forward direction
        baseYRotation = transform.eulerAngles.y;
        baseXRotation = transform.eulerAngles.x;
    }
    
    private void Update() {
        CheckSides();
        CheckForCorner();
        HandleLooking();
    }
    private void CheckSides() {
        forwardWall = Physics.Raycast(transform.position, transform.forward, rayDistance, ventWallLayer);
        leftWall = Physics.Raycast(transform.position, -transform.right, rayDistance, ventWallLayer);
        rightWall = Physics.Raycast(transform.position, transform.right, rayDistance, ventWallLayer);
        upWall = Physics.Raycast(transform.position, transform.up, rayDistance, ventWallLayer);
        downWall = Physics.Raycast(transform.position, -transform.up, rayDistance, ventWallLayer);

        Debug.DrawRay(transform.position, transform.forward * rayDistance, forwardWall ? Color.red : Color.green);
        Debug.DrawRay(transform.position, -transform.right * rayDistance, leftWall ? Color.red : Color.green);
        Debug.DrawRay(transform.position, transform.right * rayDistance, rightWall ? Color.red : Color.green);
        Debug.DrawRay(transform.position, transform.up * rayDistance, upWall ? Color.red : Color.green);
        Debug.DrawRay(transform.position, -transform.up * rayDistance, downWall ? Color.red : Color.green);
    }
    private void CheckForCorner() {
        isAtCorner = forwardWall && (leftWall || rightWall || upWall || downWall);
    }
    private void HandleLooking() {
        float mouseX = Input.GetAxis("Mouse X") * turnSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * turnSpeed;

        // Calculate rotation limits
        float leftLimit = leftWall ? 90f : horizontalLookLimit;
        float rightLimit = rightWall ? 90f : horizontalLookLimit;
        float upLimit = upWall ? 90f : verticalLookLimit;
        float downLimit = downWall ? 90f : verticalLookLimit;

        // Update rotations with clamping
        yRotation += mouseX;
        xRotation -= mouseY;

        // Apply limits relative to base forward direction
        float relativeY = Mathf.DeltaAngle(baseYRotation, yRotation);
        float relativeX = Mathf.DeltaAngle(baseXRotation, xRotation);

        // Clamp relative angles
        relativeY = Mathf.Clamp(relativeY, -leftLimit, rightLimit);
        relativeX = Mathf.Clamp(relativeX, -downLimit, upLimit);

        // Convert back to absolute angles
        yRotation = baseYRotation + relativeY;
        xRotation = baseXRotation + relativeX;

        // Apply rotations
        playerCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private void FixedUpdate() {
        HandleMovement();
    }
    private void HandleMovement() {
        rb.AddForce(Input.GetAxis("Vertical") * moveSpeed * transform.forward.normalized);
    }    
}