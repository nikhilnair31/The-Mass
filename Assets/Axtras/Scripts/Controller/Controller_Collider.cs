using UnityEngine;
using UnityEngine.Events;

public class Controller_Collider : MonoBehaviour 
{
    #region Vars
    public enum ColliderType {
        Barrier,
        VentBottom,
        VentTop,
        None
    }
    [Header("Collider Settings")]
    [SerializeField] private ColliderType colliderType;
    [SerializeField] private string showThisText;
    [SerializeField] private float showForTime = 0f;
    #endregion

    private void OnCollisionEnter(Collision other) {
        if (other.transform.CompareTag("Player")) {
            ShowText();
        }
    }
    private void OnCollisionExit(Collision other) {
        Manager_Thoughts.Instance.ClearThoughtText(true);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Player")) {
            ShowText();
        }
    }
    private void OnTriggerExit(Collider other) {
        Manager_Thoughts.Instance.ClearThoughtText(true);
    }

    private void ShowText() {
        if (
            colliderType == ColliderType.Barrier || colliderType == ColliderType.Barrier ||
            colliderType == ColliderType.VentTop
        ) {
            Manager_Thoughts.Instance.ShowText(showThisText, showForTime, true);
        }
    }
}