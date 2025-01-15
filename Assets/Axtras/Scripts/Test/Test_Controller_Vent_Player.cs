using UnityEngine;

public class Test_Controller_Vent_Player : MonoBehaviour 
{
    #region Vars
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    private Rigidbody rb;
    
    [Header("Look Settings")]
    [SerializeField] private Transform camTransform;
    [SerializeField] private float mouseSensitivity = 100f;
    private float xRotation = 0f;
    
    [Header("Ray Settings")]
    [SerializeField] private float rayDistance = 1f;
    [SerializeField] private LayerMask ventWallLayer;
    private bool forwardWall;
    private bool leftWall;
    private bool rightWall;
    private bool upWall;
    private bool downWall;
    #endregion

    private void Start() {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
        rb.useGravity = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        CheckSides();
        HandleMouseLook();
        HandleMovement();
    }
    private void HandleMouseLook() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        transform.localRotation = Quaternion.Euler(xRotation, transform.localRotation.eulerAngles.y, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    private void HandleMovement() {
        var vert = Input.GetAxis("Vertical");
        var movement = transform.forward * vert * moveSpeed * Time.deltaTime;
        rb.AddForce(movement, ForceMode.VelocityChange);
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
}