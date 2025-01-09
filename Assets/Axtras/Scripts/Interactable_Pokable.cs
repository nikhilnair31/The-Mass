using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable_Pokable : Controller_Interactables 
{
    #region Vars
    [Header("Pokable Settings")]
    [SerializeField] private float moveDist = 3f;
    #endregion

    public override void InteractInteractable(Transform currentInteractable) {
        base.InteractInteractable(currentInteractable);
        PickInteractable(currentInteractable);
    }

    private void PickInteractable(Transform interactable) {
        if (!interactable.TryGetComponent(out Rigidbody rb)) {
            rb = interactable.AddComponent<Rigidbody>();
        }

        Helper.Instance.EnablePhysics(rb, false);

        interactable.SetParent(playerController.holdAtTransform);
        
        interactable.localEulerAngles = new (90f, 0f, 0f);

        interactable.DOLocalMove(Vector3.zero, 0.5f)
        .OnComplete(() => {
            playerController.heldInteractable = interactable;
        });
    }
    private void DropInteractable(Transform interactable) {
        var rb = interactable.GetComponent<Rigidbody>();
        Helper.Instance.EnablePhysics(rb, true);
        playerController.heldInteractable.SetParent(null);
        playerController.heldInteractable = null;
    }
    public void PokableInteractable(Transform interactable) {
        if (!interactable.TryGetComponent(out Rigidbody rb)) {
            rb = interactable.AddComponent<Rigidbody>();
        }

        Helper.Instance.EnablePhysics(rb, false);

        interactable.SetParent(playerController.holdAtTransform);
        
        interactable
            .DOLocalRotate(
                interactable.localEulerAngles + new Vector3(90f, 0f, 0f), 
                0.3f
            )
            .OnComplete(() => {
                interactable
                    .DOLocalMove(
                        playerController.holdAtTransform.localPosition + new Vector3(0f, 0f, moveDist), 
                        0.3f
                    )
                    .OnComplete(() => {
                        interactable
                            .DOLocalMove(
                                Vector3.zero, 
                                0.6f
                            );
                    });
            });
    }
    
    private void OnCollisionEnter(Collision other) {
        if (other.transform.CompareTag("Mass")) {
            Controller_TheMass.Instance.GotHit();
        }

        Helper.Instance.PlayRandAudio(audioSource, audioClips);
    }
}