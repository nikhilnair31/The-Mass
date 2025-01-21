using UnityEngine;
using UnityEngine.Events;

public class Controller_Collider : MonoBehaviour 
{
    #region Vars
    [Header("Collider Settings")]
    [SerializeField] private string showThisText;
    [SerializeField] private float showForTime = 0f;
    [SerializeField] private UnityEvent invokeEvent;
    #endregion

    private void OnCollisionEnter(Collision other) {
        if (other.transform.CompareTag("Player")) {
            ShowText();
        }
    }
    private void OnCollisionStay(Collision other) {
        if (Input.GetKeyDown(KeyCode.E)) {
            invokeEvent.Invoke();
        }
    }
    private void OnCollisionExit(Collision other) {
        ClearText();
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log($"OnTriggerEnter: {other.transform.name} | {other.transform.tag}");
        if (other.transform.CompareTag("Player")) {
            ShowText();
        }
    }
    private void OnTriggerStay(Collider other) {
        Debug.Log($"OnTriggerStay: {other.transform.name} | {other.transform.tag}");
        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log($"Pressed E");
            invokeEvent.Invoke();
        }
    }
    private void OnTriggerExit(Collider other) {
        ClearText();
    }

    private void ShowText() {
        Manager_Thoughts.Instance.ShowText(
            showThisText, 
            showForTime,
            Manager_Thoughts.TextPriority.Collider
        );
    }
    private void ClearText() {
        Manager_Thoughts.Instance.ClearThoughtText(
            Manager_Thoughts.TextPriority.Collider
        );
    }
}