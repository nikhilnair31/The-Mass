using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Collections.Generic;

public class Interactable_Switch : Controller_Interactables 
{
    #region Vars
    [Header("Switch Settings")]
    [SerializeField] private Transform switchButton;
    [SerializeField] private bool isOn = true;

    [Header("Light Settings")]
    [SerializeField] private Light[] lightsList;
    [SerializeField] private List<MeshRenderer> meshRenderers = new ();
    private Material[][] matList;
    private Color[][] originalEmissionColors;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] switchClips;
    #endregion

    public virtual void Start() {
        if (meshRenderers.Count == 0) {
            meshRenderers.Add(GetComponent<MeshRenderer>());
        }

        matList = new Material[meshRenderers.Count][];
        originalEmissionColors = new Color[meshRenderers.Count][];

        for (int i = 0; i < meshRenderers.Count; i++) {
            matList[i] = meshRenderers[i].materials;
            originalEmissionColors[i] = new Color[matList[i].Length];

            for (int j = 0; j < matList[i].Length; j++) {
                if (matList[i][j].HasProperty("_EmissionColor")) {
                    originalEmissionColors[i][j] = matList[i][j].GetColor("_EmissionColor");
                }
            }
        }

        if (switchButton != null) {
            switchButton.localRotation = Quaternion.Euler(60f, 0f, 0f);
        }

        ControlOnOffLight();
    }

    public void ControlOnOffLight() {
        if (switchButton != null) {
            float targetXRotation = isOn ? 60f : -60f;
            switchButton.DOLocalRotate(new Vector3(targetXRotation, 0f, 0f), 0.5f);
        }
        
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
        for (int i = 0; i < meshRenderers.Count; i++) {
            for (int j = 0; j < matList[i].Length; j++) {
                var material = matList[i][j];

                if (isOn) {
                    material.SetColor("_EmissionColor", originalEmissionColors[i][j]);
                    material.EnableKeyword("_EMISSION");
                }
                else {
                    material.SetColor("_EmissionColor", Color.black);
                    material.DisableKeyword("_EMISSION");
                }
            }
        }
    }
}