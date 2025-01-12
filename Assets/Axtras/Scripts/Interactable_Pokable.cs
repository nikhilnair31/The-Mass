using DG.Tweening;
using UnityEngine;

public class Interactable_Pokable : Controller_Pickable 
{
    #region Vars
    [Header("Pokable Settings")]
    [SerializeField] private Vector3 moveDir;
    private Vector3 initPos;
    private Vector3 initRot;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] impactClips;
    #endregion

    public override void Start() {
        base.Start();

        rgb.isKinematic = false;
        rgb.useGravity = true;
        
        initPos = transform.localPosition;
        initRot = transform.localEulerAngles;
    }

    public void PokableInteractable() {
        // var initPos = transform.localPosition;
        // var initRot = transform.localEulerAngles;
        transform
            .DOLocalRotate(
                initRot + new Vector3(90f, 0f, 0f), 
                0.3f
            )
            .OnComplete(() => {
                transform
                    .DOLocalMove(
                        initPos + moveDir, 
                        0.3f
                    )
                    .OnComplete(() => {
                        transform
                            .DOLocalMove(
                                initPos, 
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
        // Debug.Log($"{transform.name} - OnCollisionEnter - {other.transform.name} - {other.transform.tag}");
        if (other.transform.CompareTag("Mass")) {
            Controller_TheMass.Instance.GotHit();
        }

        Helper.Instance.PlayRandAudio(audioSource, impactClips);
    }
}