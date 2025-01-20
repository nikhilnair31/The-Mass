using System.Collections;
using UnityEngine;

public class Interactable_Torch : Controller_Pickable 
{
    #region Vars
    [Header("Light Settings")]
    [SerializeField] private bool isOn = true;
    [SerializeField] private Light[] lightsList;

    [Header("Flicker Settings")]
    [SerializeField] private float minFlickerInterval = 0.05f;
    [SerializeField] private float maxFlickerInterval = 0.3f;
    [SerializeField] private float flickerChance = 0.3f;
    private Coroutine flickerCoroutine;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] switchClips;
    #endregion

    public override void Start() {
        InitializeSwitchState(isOn);
    } 
    private void InitializeSwitchState(bool initialState) {
        isOn = initialState;
        LightsControl();
        StartCoroutine(FlickerLights());
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

    private IEnumerator FlickerLights() {
        while (true) {
            float flickerInterval = Random.Range(minFlickerInterval, maxFlickerInterval);

            foreach (var light in lightsList) {
                light.enabled = Random.value > flickerChance && isOn;
            }

            yield return new WaitForSeconds(flickerInterval);
        }
    }
}