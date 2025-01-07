using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable_Throwable : Controller_Interactables 
{
    #region Vars
    [Header("Throwable Settings")]
    [SerializeField] private float throwForce = 3f;
    #endregion

    public override void InteractInteractable(Transform currentInteractable) {
        base.InteractInteractable(currentInteractable);
        PickInteractable(currentInteractable);
    }

    private void PickInteractable(Transform interactable) {
        if (!interactable.TryGetComponent(out Rigidbody rb)) {
            rb = interactable.AddComponent<Rigidbody>();
        }

        EnablePhysics(rb, false);

        interactable.SetParent(playerController.holdAtTransform);

        interactable.DOLocalMove(Vector3.zero, 0.5f)
        .OnComplete(() => {
            playerController.heldInteractable = interactable;
        });
    }
    public void DropInteractable(Transform interactable) {
        var rb = interactable.GetComponent<Rigidbody>();
        EnablePhysics(rb, true);
        playerController.heldInteractable.SetParent(null);
        playerController.heldInteractable = null;
    }
    public void ThrowInteractable(Transform interactable) {
        var rb = interactable.GetComponent<Rigidbody>();
        EnablePhysics(rb, true);
        rb.AddForce(GetDir(interactable) * throwForce, ForceMode.Impulse);
        playerController.heldInteractable.SetParent(null);
        playerController.heldInteractable = null;
    }

    private Vector3 GetDir(Transform interactable) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            return (hit.point - interactable.position).normalized;
        }
        else {
            return ray.direction;
        }
    }

    private void EnablePhysics(Rigidbody rb, bool active) {
        rb.useGravity = active;
        rb.isKinematic = !active;
    }

    private void OnCollisionEnter(Collision other) {
        PlayRandAudio(audioSource, audioClips);
    }
}