using UnityEngine;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;

public class Controller_Player : MonoBehaviour
{
    #region Vars
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
    private bool aimingToThrow;
    private RaycastHit hit;
    #endregion

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        HandleMouseLook();
        HandleMovement();
        HandleInteractable();
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
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E)) {
            var interactable = currentInteractable.GetComponent<Controller_Interactables>();
            interactable?.InteractInteractable(currentInteractable);
        }

        if (Input.GetKeyDown(KeyCode.G) && heldInteractable != null) {
            var throwable = heldInteractable.GetComponent<Interactable_Throwable>();
            throwable?.DropInteractable(heldInteractable);
        }
        
        if (Input.GetMouseButtonDown(0) && heldInteractable != null) {
            aimingToThrow = true;
            throwLineGO.SetActive(true);
        }
        if (Input.GetMouseButtonUp(0) && heldInteractable != null) {
            var throwable = heldInteractable.GetComponent<Interactable_Throwable>();
            throwable?.ThrowInteractable(heldInteractable);
            throwLineGO.SetActive(false);
            aimingToThrow = false;
        }
    }

    private void CheckForInteractable() {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, maxDistance, interactableLayer.value)) {
            if (hit.transform != currentInteractable) {
                if (hit.transform.TryGetComponent(out Controller_Interactables interactable)) {
                    currentInteractable = hit.transform;
                    UpdateLookedAtText(interactable);
                }
            }
        }
        else {
            ClearLookedAtText();
        }
    }
    private void UpdateLookedAtText(Controller_Interactables interactable) {
        var showTextStr = interactable.ReturnInteractableText();

        StopAllCoroutines();
        StartCoroutine(Manager_UI.Instance.ShowTextWithSound(showTextStr));
    }
    private void ClearLookedAtText() {
        if (currentInteractable != null) {
            currentInteractable = null;
            StartCoroutine(Manager_UI.Instance.ClearText());
        }
    }
}
