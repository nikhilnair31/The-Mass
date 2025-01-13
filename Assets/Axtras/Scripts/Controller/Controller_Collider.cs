using UnityEngine;

public class Controller_Collider : MonoBehaviour 
{
    #region Vars
    public enum ColliderType {
        Barrier,
        VentBottom,
        VentTop
    }
    [Header("Collider Settings")]
    [SerializeField] private ColliderType colliderType;
    [SerializeField] private string firstTextStr;
    [SerializeField] private string secondTextStr;
    #endregion

    private void OnCollisionEnter(Collision other) {
        if (colliderType == ColliderType.Barrier) {
            if (other.transform.CompareTag("Player")) {
                var isPlayersFirstColl = PlayerPrefs.GetInt("IsPlayersFirstColl");
                if (isPlayersFirstColl == 1) {
                    StopAllCoroutines();
                    Manager_Thoughts.Instance.UpdateThoughtText(firstTextStr);
                    PlayerPrefs.SetInt("IsPlayersFirstColl", 0);
                }
                else {
                    StopAllCoroutines();
                    Manager_Thoughts.Instance.UpdateThoughtText(secondTextStr);
                }
            }
            if (other.transform.CompareTag("Interactable")) {
                // ???
            }
        }
        else if (colliderType == ColliderType.VentBottom) {
            if (other.transform.CompareTag("Player")) {
                var allAttemptsCompleted = Manager_Game.Instance.GetIfAllAttemptsCompleted();

                StopAllCoroutines();
                Manager_Thoughts.Instance.UpdateThoughtText(
                    allAttemptsCompleted ? firstTextStr : "Error"
                );
            }
        }
        else if (colliderType == ColliderType.VentTop) {
            if (other.transform.CompareTag("Player")) {
                Debug.Log($"Starting end sequence...");
            }
        }
    }
    private void OnCollisionExit(Collision other) {
        Manager_Thoughts.Instance.ClearThoughtText();
    }
}