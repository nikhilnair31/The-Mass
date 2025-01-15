using UnityEngine;
using DG.Tweening;

public class Interactable_Blinds : Controller_Interactables 
{
    #region Vars
    [Header("Blinds Settings")]
    [SerializeField] private float transitionTime = 1f;
    private Vector3 startScale;
    private bool isDown = true;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] opencloseClips;
    #endregion

    public virtual void Start() {
        rgb.isKinematic = true;
        rgb.useGravity = false;

        startScale = transform.localScale;
    }

    public void OpenCloseBlinds() {
        Debug.Log($"OpenCloseBlinds");
        
        transform.DOScale(isDown ? new (startScale.x, startScale.y, 0.05f) : startScale, transitionTime);
        
        Helper.Instance.PlayRandAudio(audioSource, opencloseClips);

        isDown = !isDown;
    }
}