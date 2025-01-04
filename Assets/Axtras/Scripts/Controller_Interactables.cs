using UnityEngine;

public class Controller_Interactables : MonoBehaviour 
{
    #region Vars
    [Header("Interaction Settings")]
    [SerializeField] private string showThisText;
    #endregion    

    public string ShowInteractablesText() {
        return showThisText;
    }
}