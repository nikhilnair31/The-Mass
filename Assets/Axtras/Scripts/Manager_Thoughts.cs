using UnityEngine;

public class Manager_Thoughts : MonoBehaviour 
{
    #region Vars
    public static Manager_Thoughts Instance { get; private set; }
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateThoughtText(Controller_Interactables interactable = null) {
        var showTextStr = interactable.ReturnInteractableText();

        StopAllCoroutines();
        StartCoroutine(Manager_UI.Instance.ShowTextWithSound(showTextStr));
    }
    public void ClearThoughtText() {
        StopAllCoroutines();
        StartCoroutine(Manager_UI.Instance.ClearText());
    }    
}