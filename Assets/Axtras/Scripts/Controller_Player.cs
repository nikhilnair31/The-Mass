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
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody rb;
    private Vector3 moveDirection;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform playerCamera;
    private float xRotation = 0f;

    [Header("Interactable Settings")]
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform holdAtTransform;
    [SerializeField] private GameObject throwLineGO;
    [SerializeField] private float throwForce = 3f;
    private Transform currentInteractable;
    private Transform heldInteractable;
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

        if (Input.GetKey(KeyCode.LeftShift)) {
            rb.linearVelocity = new Vector3(moveDirection.x * sprintSpeed, rb.linearVelocity.y, moveDirection.z * sprintSpeed);
        }
        else {
            rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
        }

        rb.AddForce(Vector3.up * gravity);
    }
    private void HandleInteractable() {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E)) {
            if (currentInteractable != null && currentInteractable.TryGetComponent(out Controller_Interactables interactable)) {
                if (interactable.ReturnInteractableBool()) {
                    interactable.Interacted();
                }

                if (interactable.ReturnPickableBool() && heldInteractable == null) {
                    PickInteractable(currentInteractable);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && heldInteractable != null) {
            DropInteractable(heldInteractable);
        }
        
        if (Input.GetMouseButtonDown(0) && heldInteractable != null) {
            aimingToThrow = true;
            throwLineGO.SetActive(true);
        }
        if (Input.GetMouseButtonUp(0) && heldInteractable != null) {
            ThrowInteractable(heldInteractable);
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

    private void PickInteractable(Transform interactable) {
        if (!interactable.TryGetComponent(out Rigidbody rb)) {
            rb = interactable.AddComponent<Rigidbody>();
        }

        EnablePhysics(rb, false);

        interactable.SetParent(holdAtTransform);

        interactable.DOLocalMove(Vector3.zero, 0.5f)
        .OnComplete(() => {
            heldInteractable = interactable;
        });
    }
    private void DropInteractable(Transform interactable) {
        var rb = interactable.GetComponent<Rigidbody>();
        EnablePhysics(rb, true);
        heldInteractable.SetParent(null);
        heldInteractable = null;
    }
    private void ThrowInteractable(Transform interactable) {
        var rb = interactable.GetComponent<Rigidbody>();
        EnablePhysics(rb, true);
        rb.AddForce(holdAtTransform.forward * throwForce, ForceMode.Impulse);
        heldInteractable.SetParent(null);
        heldInteractable = null;
    }

    private void EnablePhysics(Rigidbody rb, bool active) {
        rb.useGravity = active;
        rb.isKinematic = !active;
    }
}
