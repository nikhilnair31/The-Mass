using UnityEngine;
using DG.Tweening;

public class Interactable_Switch : Controller_Interactables 
{
    #region Vars
    [Header("Switch Settings")]
    [SerializeField] private Transform switchButton;
    [SerializeField] private Light lightToControl;
    private bool isOn = false;
    #endregion

    public override void Start() {
        base.Start();
        switchButton.localRotation = Quaternion.Euler(60f, 0f, 0f);
    }

    public override void InteractInteractable(Transform currentInteractable) {
        base.InteractInteractable(currentInteractable);

        float targetXRotation = isOn ? -60f : 60f;
        switchButton.DORotate(new Vector3(targetXRotation, 0f, 0f), 0.5f);
        
        Helper.Instance.PlayRandAudio(audioSource, audioClips);

        lightToControl.enabled = isOn;

        isOn = !isOn;
    }
}