using UnityEngine;

public class Controller_Interactables : MonoBehaviour 
{
    #region Vars
    [Header("Interaction Settings")]
    [SerializeField] private string showThisText;
    [SerializeField] private bool canBeInteracted;
    [SerializeField] private bool canBePicked;
    #endregion

    private void Start() {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public virtual void InteractInteractable() {
        // Debug.Log("Controller_Interactables InteractInteractable");
    }

    internal void PlayRandAudio(AudioSource source, AudioClip[] clips) {
        var clip = clips[Random.Range(0, clips.Length)];
        source.PlayOneShot(clip);
    }

    public string ReturnInteractableText() {
        return showThisText;
    }
    public bool ReturnInteractableBool() {
        return canBeInteracted;
    }
    public bool ReturnPickableBool() {
        return canBePicked;
    }
}