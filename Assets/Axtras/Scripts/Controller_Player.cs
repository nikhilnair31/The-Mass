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
    private CharacterController controller;
    private Vector3 velocity;

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

    [Header("UI Settings")]
    [SerializeField] private TMP_Text lookedAtText;
    #endregion

    private void Start() {
        controller = GetComponent<CharacterController>();
        
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

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            controller.Move(move * sprintSpeed * Time.deltaTime);
        }
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
        
        if (Input.GetMouseButtonDown(0)) {
            aimingToThrow = true;
            throwLineGO.SetActive(true);
        }
        if (Input.GetMouseButtonUp(0)) {
            ThrowInteractable(heldInteractable);
            throwLineGO.SetActive(false);
            aimingToThrow = false;
        }
    }

    private void CheckForInteractable() {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, maxDistance, interactableLayer)) {
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
        lookedAtText.text = interactable.ReturnInteractableText();
        Helper.Instance.ScaleTween(lookedAtText.transform, 3f);
    }
    private void ClearLookedAtText() {
        if (currentInteractable != null) {
            currentInteractable = null;
            Helper.Instance.ScaleTween(lookedAtText.transform, 0.3f);
        }
    }

    private void PickInteractable(Transform interactable) {
        if (!interactable.TryGetComponent(out Rigidbody rb)) {
            interactable.AddComponent<Rigidbody>();
        }

        interactable.DOMove(holdAtTransform.position, 0.5f)
        .OnComplete(() => {
            var rb = interactable.GetComponent<Rigidbody>();
            EnablePhysics(rb, false);

            interactable.SetParent(holdAtTransform);

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
