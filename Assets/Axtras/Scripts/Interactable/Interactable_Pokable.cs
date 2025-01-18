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
                playerController.ControlCanMoveAndLook(false);
                initPos = transform.localPosition;
                initRot = transform.localEulerAngles;
            })
            .Append(transform.DOLocalRotate(endRot, 0.4f))
            .Append(transform.DOShakeRotation(1f, 3f, 10, 90f))
            .Append(transform.DOLocalMove(endPos, 0.15f))
            .AppendInterval(1f)
            .Append(transform.DOLocalRotate(initRot, 0.5f))
            .Join(transform.DOLocalMove(initPos, 0.5f))
            .OnComplete(() => {
                playerController.ControlCanMoveAndLook(true);
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