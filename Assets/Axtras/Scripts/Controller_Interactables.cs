using UnityEngine;

public class Controller_Interactables : MonoBehaviour 
{
    #region Vars
    internal Controller_Player playerController;

    [Header("Interaction Settings")]
    [SerializeField] private string showThisText;
    [SerializeField] private bool canBePicked;
    #endregion

    private void Start() {
        playerController = FindFirstObjectByType<Controller_Player>();
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public virtual void InteractInteractable(Transform currentInteractable) {
        // Debug.Log("Controller_Interactables InteractInteractable");
    }

    internal void PlayRandAudio(AudioSource source, AudioClip[] clips) {
        var clip = clips[Random.Range(0, clips.Length)];
        source.PlayOneShot(clip);
    }

    public string ReturnInteractableText() {
        return showThisText;
    }
    public bool ReturnPickableBool() {
        return canBePicked;
    }
}