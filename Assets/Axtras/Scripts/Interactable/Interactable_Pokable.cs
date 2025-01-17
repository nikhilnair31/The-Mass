using DG.Tweening;
using UnityEngine;

public class Interactable_Pokable : Controller_Pickable 
{
    #region Vars
    [Header("Pokable Settings")]
    [SerializeField] private Vector3 endPos;
    [SerializeField] private Vector3 endRot;
    private Sequence pokableSequence;
    private Vector3 initPos;
    private Vector3 initRot;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] impactClips;
    #endregion

    public override void Start() {
        base.Start();

        rgb.isKinematic = false;
        rgb.useGravity = true;
    }

    public void PokableInteractable() {
        if (pokableSequence != null) return;

        pokableSequence = DOTween.Sequence();
        pokableSequence
            .OnStart(() => {
                initPos = transform.localPosition;
                initRot = transform.localEulerAngles;
            })
            .Append(transform.DOLocalRotate(endRot, 0.6f))
            .Append(transform.DOShakeRotation(1f, 2f, 10, 90f))
            .Append(transform.DOLocalMove(endPos, 0.2f))
            .AppendInterval(2f)
            .Append(transform.DOLocalRotate(initRot, 1f))
            .Join(transform.DOLocalMove(initPos, 1f))
            .OnComplete(() => {
                pokableSequence = null;
            });
        ;
    }
    
    private void OnCollisionEnter(Collision other) {
        // Should have sufficient speed
        // if (rgb.linearVelocity.magnitude < 0.1f) return;

        Debug.Log($"OnCollisionEnter transform.name: {transform.name} - linearVelocity: {rgb.linearVelocity.magnitude}");

        if (other.transform.CompareTag("Mass")) {
            Controller_TheMass.Instance.GotHit("Pokable");
        }

        Helper.Instance.PlayRandAudio(audioSource, impactClips);
    }
}