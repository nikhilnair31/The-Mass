using DG.Tweening;
using UnityEngine;

public class Interactable_Pokable : Controller_Pickable 
{
    #region Vars
    [Header("Pokable Settings")]
    [SerializeField] private float moveDist = 3f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] impactClips;
    #endregion

    public void PokableInteractable() {
        transform
            .DOLocalRotate(
                transform.localEulerAngles + new Vector3(90f, 0f, 0f), 
                0.3f
            )
            .OnComplete(() => {
                transform
                    .DOLocalMove(
                        playerController.holdAtTransform.localPosition + new Vector3(0f, 0f, moveDist), 
                        0.3f
                    )
                    .OnComplete(() => {
                        transform
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

        Helper.Instance.PlayRandAudio(audioSource, impactClips);
    }
}