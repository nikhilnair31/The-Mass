using UnityEngine;

public class Controller_Interactables : MonoBehaviour 
{
    #region Vars
    internal Controller_Player playerController;

    [Header("Component Settings")]
    internal AudioSource audioSource;
    internal BoxCollider coll;
    internal Rigidbody rgb;

    [Header("Interaction Settings")]
    [SerializeField] internal string showThisText;
    #endregion

    public virtual void Start() {
        playerController = FindFirstObjectByType<Controller_Player>();

        if (!transform.TryGetComponent(out AudioSource source)) {
            audioSource = transform.gameObject.AddComponent<AudioSource>();
        } 
        else {
            audioSource = source;
        }
        if (!transform.TryGetComponent(out BoxCollider bc)) {
            coll = transform.gameObject.AddComponent<BoxCollider>();
        } 
        else {
            coll = bc;
        }
        if (!transform.TryGetComponent(out Rigidbody rb)) {
            rgb = transform.gameObject.AddComponent<Rigidbody>();
        } 
        else {
            rgb = rb;
        }

        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void SetInteractionText(string newtext) {
        showThisText = newtext;
    }

    public string ReturnInteractableText() {
        return showThisText;
    }
}