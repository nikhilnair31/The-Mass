using UnityEngine;

public class Interactable_Torch : Controller_Pickable 
{
    #region Vars
    [Header("Light Settings")]
    [SerializeField] private bool isOn = true;
    [SerializeField] private Light[] lightsList;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] switchClips;
    #endregion

    public override void Start() {
        InitializeSwitchState(isOn);
    } 
    private void InitializeSwitchState(bool initialState) {
        isOn = initialState;
        LightsControl();
    }

    public void ToggleSwitch() {
        isOn = !isOn;
        LightsControl();
        Helper.Instance.PlayRandAudio(audioSource, switchClips);
    }
    private void LightsControl() {
        foreach (var light in lightsList) {
            light.enabled = isOn;
        }
    }
}