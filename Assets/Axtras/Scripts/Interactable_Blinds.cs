using UnityEngine;
using DG.Tweening;

public class Interactable_Blinds : Controller_Interactables 
{
    #region Vars
    [Header("Blinds Settings")]
    [SerializeField] private float transitionTime = 1f;
    private Vector3 startScale;
    private bool isDown = true;
    #endregion

    public override void Start() {
        base.Start();
        startScale = transform.localScale;
    }

    public override void InteractInteractable(Transform currentInteractable) {
        base.InteractInteractable(currentInteractable);
        OpenCloseBlinds();
    }
    private void OpenCloseBlinds() {
        transform.DOScale(isDown ? new (startScale.x, startScale.y, 0.05f) : startScale, transitionTime);
        
        PlayRandAudio(audioSource, audioClips);

        isDown = !isDown;
    }
}