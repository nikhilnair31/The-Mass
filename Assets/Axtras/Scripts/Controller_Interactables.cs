using UnityEngine;

public class Controller_Interactables : MonoBehaviour 
{
    #region Vars
    internal Controller_Player playerController;

    [Header("Component Settings")]
    internal AudioSource audioSource;

    [Header("Interaction Settings")]
    [SerializeField] private string showThisText;
    #endregion

    public virtual void Start() {
        playerController = FindFirstObjectByType<Controller_Player>();

        audioSource = GetComponent<AudioSource>();

        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void SetInteractionText(string newtext) {
        showThisText = newtext;
    }

    public string ReturnInteractableText() {
        return showThisText;
    }
}