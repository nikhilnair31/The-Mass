using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable_Throwable : Controller_Interactables 
{
    #region Vars
    [Header("Throwable Settings")]
    [SerializeField] private float throwForce = 3f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource impactAudioSource;
    [SerializeField] private AudioClip[] impactAudioClips;
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
        rb.AddForce(playerController.holdAtTransform.forward * throwForce, ForceMode.Impulse);
        playerController.heldInteractable.SetParent(null);
        playerController.heldInteractable = null;
    }

    private void EnablePhysics(Rigidbody rb, bool active) {
        rb.useGravity = active;
        rb.isKinematic = !active;
    }

    private void OnCollisionEnter(Collision other) {
        PlayRandAudio(impactAudioSource, impactAudioClips);
    }
}