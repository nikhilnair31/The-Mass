using DG.Tweening;
using UnityEngine;

public class Interactable_Switch : Controller_Interactables 
{
    #region Vars
    [Header("Interaction Settings")]
    [SerializeField] private Transform switchButton;
    [SerializeField] private AudioSource switchAudioSource;
    [SerializeField] private AudioClip[] switchAudioClips;
    [SerializeField] private Light lightToControl;
    private bool isOn = false;
    #endregion

    private void Start() {
        switchButton.localRotation = Quaternion.Euler(60f, 0f, 0f);
    }

    public override void Interacted() {
        base.Interacted();
        Debug.Log("Interactable_Switch Interacted");

        float targetXRotation = isOn ? -60f : 60f;
        switchButton.DORotate(new Vector3(targetXRotation, 0f, 0f), 0.5f);
        
        var clip = switchAudioClips[Random.Range(0, switchAudioClips.Length)];
        switchAudioSource.PlayOneShot(clip);

        lightToControl.enabled = isOn;

        isOn = !isOn;
    }
}