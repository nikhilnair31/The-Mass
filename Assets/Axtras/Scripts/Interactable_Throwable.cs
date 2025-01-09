using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable_Throwable : Controller_Interactables 
{
    #region Vars
    [Header("Throwable Settings")]
    [SerializeField] private float throwForce = 3f;

    [Header("Breakable Settings")]
    [SerializeField] private bool isBreakable = false;
    [SerializeField] private GameObject shatteredPrefab;
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private float explosionRadius = 1f;
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

        interactable.DOLocalMove(Vector3.zero, 0.5f)
        .OnComplete(() => {
            playerController.heldInteractable = interactable;
        });
    }
    public void DropInteractable(Transform interactable) {
        var rb = interactable.GetComponent<Rigidbody>();
        Helper.Instance.EnablePhysics(rb, true);
        playerController.heldInteractable.SetParent(null);
        playerController.heldInteractable = null;
    }
    public void ThrowInteractable(Transform interactable) {
        var rb = interactable.GetComponent<Rigidbody>();
        
        Helper.Instance.EnablePhysics(rb, true);
        var dir = Helper.Instance.GetDir(interactable);
        rb.AddForce(dir * throwForce, ForceMode.Impulse);
        
        playerController.heldInteractable.SetParent(null);
        playerController.heldInteractable = null;
    }

    private void OnCollisionEnter(Collision other) {
        if (isBreakable) {
            var spawn = Instantiate(shatteredPrefab, transform.position, Quaternion.identity);
            foreach (var rb in spawn.GetComponentsInChildren<Rigidbody>()) {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            gameObject.SetActive(false);
            
            if (other.transform.CompareTag("Mass")) {
                Controller_TheMass.Instance.GotHit();
            }
        }

        Helper.Instance.PlayRandAudio(audioSource, audioClips);
    }
}