using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
    private void OnCollisionExit(Collision other) {
        ClearText();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Player")) {
            ShowText();
        }
    }
    private void OnTriggerStay(Collider other) {
        if (Input.GetKeyDown(KeyCode.E)) {
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
            true
        );
    }
    private void ClearText() {
        Manager_Thoughts.Instance.ClearThoughtText(true);
    }
}