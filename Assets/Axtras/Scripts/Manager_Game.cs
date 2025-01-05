using UnityEngine;

public class Manager_Game : MonoBehaviour 
{
    #region Vars
    [Header("Unlock Vent Settings")]
    [SerializeField] private int numOfAttemptsCompleted = 5;
    [SerializeField] private int numOfAttemptsAttempted;
    #endregion

    private void AddAttempt() {
        numOfAttemptsAttempted++;

        if (numOfAttemptsAttempted >= numOfAttemptsCompleted) {
            UnlockVent();
        }
    }

    private void UnlockVent() {
        Debug.Log($"UnlockVent");
    }
}