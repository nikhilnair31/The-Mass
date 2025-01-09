using UnityEngine;
using DG.Tweening;

public class Interactable_Switch : Controller_Interactables 
{
    #region Vars
    [Header("Switch Settings")]
    [SerializeField] private Transform switchButton;
    [SerializeField] private Light[] lightsList;
    private bool isOn = false;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] switchClips;
    #endregion

    public override void Start() {
        base.Start();
        
        switchButton.localRotation = Quaternion.Euler(60f, 0f, 0f);
    }

    public void ControlOnOffLight() {
        float targetXRotation = isOn ? -60f : 60f;
        switchButton.DORotate(new Vector3(targetXRotation, 0f, 0f), 0.5f);
        
        Helper.Instance.PlayRandAudio(audioSource, switchClips);

        LightsControl();

        isOn = !isOn;
    }
    private void LightsControl() {
        foreach (var light in lightsList) {
            light.enabled = isOn;
        }
    }
}