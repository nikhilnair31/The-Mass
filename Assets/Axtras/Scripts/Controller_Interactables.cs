using UnityEngine;

public class Controller_Interactables : MonoBehaviour 
{
    #region Vars
    internal Controller_Player playerController;

    [Header("Interaction Settings")]
    [SerializeField] private string showThisText;
    [SerializeField] public bool canBePicked;

    [Header("Audio Settings")]
    [SerializeField] internal AudioClip[] audioClips;
    internal AudioSource audioSource;
    #endregion

    public virtual void Start() {
        playerController = FindFirstObjectByType<Controller_Player>();

        audioSource = GetComponent<AudioSource>();

        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public virtual void InteractInteractable(Transform currentInteractable) {
        // Debug.Log("Controller_Interactables InteractInteractable");
    }

    public void SetInteractionText(string newtext) {
        showThisText = newtext;
    }

    public string ReturnInteractableText() {
        return showThisText;
    }
    public bool ReturnPickableBool() {
        return canBePicked;
    }
}