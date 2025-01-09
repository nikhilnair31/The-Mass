using UnityEngine;
using DG.Tweening;

public class Interactable_Switch : Controller_Interactables 
{
    #region Vars
    [Header("Switch Settings")]
    [SerializeField] private Transform switchButton;
    private bool isOn = false;

    [Header("Light Settings")]
    [SerializeField] private Light[] lightsList;
    private MeshRenderer meshRenderer;
    private Material[] matList;
    private Color[] originalEmissionColors;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] switchClips;
    #endregion

    public override void Start() {
        base.Start();
        
        // Store original emission colors
        meshRenderer = GetComponent<MeshRenderer>();
        matList = meshRenderer.materials;
        originalEmissionColors = new Color[matList.Length];
        for (int i = 0; i < matList.Length; i++) {
            originalEmissionColors[i] = matList[i].GetColor("_EmissionColor");
        }

        // Set the initial switch button rotation
        switchButton.localRotation = Quaternion.Euler(60f, 0f, 0f);
    }

    public void ControlOnOffLight() {
        float targetXRotation = isOn ? -60f : 60f;
        switchButton.DOLocalRotate(new Vector3(targetXRotation, 0f, 0f), 0.5f);
        
        Helper.Instance.PlayRandAudio(audioSource, switchClips);

        EmissionControl();
        LightsControl();

        isOn = !isOn;
    }
    private void LightsControl() {
        foreach (var light in lightsList) {
            light.enabled = isOn;
        }
    }
    private void EmissionControl() {
        for (int i = 0; i < matList.Length; i++) {
            var material = matList[i];

            if (isOn) {
                // Reset to the original emission color
                material.SetColor("_EmissionColor", originalEmissionColors[i]);
                material.EnableKeyword("_EMISSION");
            } 
            else {
                // Set emission to black (off)
                material.SetColor("_EmissionColor", Color.black);
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}