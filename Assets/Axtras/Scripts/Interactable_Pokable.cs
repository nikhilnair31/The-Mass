using DG.Tweening;
using UnityEngine;

public class Interactable_Pokable : Controller_Pickable 
{
    #region Vars
    [Header("Pokable Settings")]
    [SerializeField] private Vector3 moveDir;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] impactClips;
    #endregion

    public void PokableInteractable() {
        var initRot = transform.localEulerAngles;
        transform
            .DOLocalRotate(
                transform.localEulerAngles + new Vector3(90f, 0f, 0f), 
                0.3f
            )
            .OnComplete(() => {
                transform
                    .DOLocalMove(
                        transform.localPosition + moveDir, 
                        0.3f
                    )
                    .OnComplete(() => {
                        transform
                            .DOLocalMove(
                                Vector3.zero, 
                                0.6f
                            );
                        transform
                            .DOLocalRotate(
                                initRot, 
                                0.3f
                            );
                    });
            });
    }
    
    private void OnCollisionEnter(Collision other) {
        Debug.Log($"{transform.name} - OnCollisionEnter - {other.transform.name} - {other.transform.tag}");
        if (other.transform.CompareTag("Mass")) {
            Controller_TheMass.Instance.GotHit();
        }

        Helper.Instance.PlayRandAudio(audioSource, impactClips);
    }
}