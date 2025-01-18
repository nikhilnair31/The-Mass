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
        Debug.Log($"OnCollisionEnter\nother.transform.tag: {other.transform.tag}\ncolliderType: {colliderType}");
        if (other.transform.CompareTag("Player")) {
            if (
                colliderType == ColliderType.Barrier || colliderType == ColliderType.Barrier ||
                colliderType == ColliderType.VentTop
            ) {
                Manager_Thoughts.Instance.ShowText(showThisText, showForTime, true);
            }
        }
    }
    private void OnCollisionExit(Collision other) {
        Manager_Thoughts.Instance.ClearThoughtText(true);
    }
}