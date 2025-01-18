using UnityEngine;

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
            if (
                colliderType == ColliderType.Barrier || colliderType == ColliderType.Barrier ||
                colliderType == ColliderType.VentTop
            ) {
                StartCoroutine(
                    Manager_Thoughts.Instance.ShowTextSequence(showThisText, showForTime)
                );
            }
        }
    }
    private void OnCollisionExit(Collision other) {
        Manager_Thoughts.Instance.ClearThoughtText();
    }
}