using UnityEngine;

public class Interactable_Throwable : Controller_Pickable 
{
    #region Vars
    [Header("Throwable Settings")]
    [SerializeField] private float throwForce = 3f;

    [Header("Breakable Settings")]
    [SerializeField] private bool isBreakable = false;
    [SerializeField] private GameObject shatteredPrefab;
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private float explosionRadius = 1f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] impactClips;
    #endregion

    public void ThrowInteractable() {
        var rb = transform.GetComponent<Rigidbody>();
        Helper.Instance.EnablePhysics(rb, true);
        var dir = Helper.Instance.GetDir(transform);
        rb.AddForce(dir * throwForce, ForceMode.Impulse);
        
        playerController.heldInteractable.SetParent(null);
        playerController.heldInteractable = null;
    }

    private void OnCollisionEnter(Collision other) {
        if (isBreakable && wasHeld) {
            var spawn = Instantiate(shatteredPrefab, transform.position, Quaternion.identity);
            foreach (ContactPoint contact in other.contacts) {
                Vector3 explosionPoint = contact.point;
                Vector3 explosionDirection = contact.normal;

                foreach (var rb in spawn.GetComponentsInChildren<Rigidbody>()) {
                    rb.AddExplosionForce(explosionForce, explosionPoint, explosionRadius, upwardsModifier: 0f, ForceMode.Impulse);
                    rb.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);
                }

                break;
            }

            gameObject.SetActive(false);
            
            if (other.transform.CompareTag("Mass")) {
                Controller_TheMass.Instance.GotHit();
            }
        }

        Helper.Instance.PlayRandAudio(audioSource, impactClips);
    }
}