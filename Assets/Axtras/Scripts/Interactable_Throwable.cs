using UnityEngine;

public class Interactable_Throwable : Controller_Interactables 
{
    #region Vars
    [Header("Switch Settings")]
    [SerializeField] private AudioSource impactAudioSource;
    [SerializeField] private AudioClip[] impactAudioClips;
    #endregion

    private void OnCollisionEnter(Collision other) {
        PlayRandAudio(impactAudioSource, impactAudioClips);
    }
}