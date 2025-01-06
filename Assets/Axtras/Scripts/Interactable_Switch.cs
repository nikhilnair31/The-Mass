using UnityEngine;
using DG.Tweening;

public class Interactable_Switch : Controller_Interactables 
{
    #region Vars
    [Header("Switch Settings")]
    [SerializeField] private Transform switchButton;
    [SerializeField] private AudioSource switchAudioSource;
    [SerializeField] private AudioClip[] switchAudioClips;
    [SerializeField] private Light lightToControl;
    private bool isOn = false;
    #endregion

    private void Start() {
        switchButton.localRotation = Quaternion.Euler(60f, 0f, 0f);
    }

    public override void InteractInteractable() {
        base.InteractInteractable();
        Debug.Log("Interactable_Switch InteractInteractable");

        float targetXRotation = isOn ? -60f : 60f;
        switchButton.DORotate(new Vector3(targetXRotation, 0f, 0f), 0.5f);
        
        PlayRandAudio(switchAudioSource, switchAudioClips);

        lightToControl.enabled = isOn;

        isOn = !isOn;
    }
}