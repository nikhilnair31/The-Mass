using UnityEngine;
using TMPro;

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
    [SerializeField] private LayerMask layerMask;
    private Transform currentInteractable;
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
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, maxDistance, layerMask)) {
            if (hit.transform != currentInteractable) {
                if (hit.transform.TryGetComponent(out Controller_Interactables interactable)) {
                    currentInteractable = hit.transform;
                    lookedAtText.text = interactable.ShowInteractablesText();
                    Helper.Instance.ScaleTween(lookedAtText.transform, 3f);
                }
            }
        }
        else {
            if (currentInteractable != null) {
                currentInteractable = null;
                Helper.Instance.ScaleTween(lookedAtText.transform, 0.3f);
            }
        }
    }
}
