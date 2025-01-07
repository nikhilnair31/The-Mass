using UnityEngine;

public class Manager_Game : MonoBehaviour 
{
    #region Vars
    public static Manager_Game Instance { get; private set; }

    [Header("Unlock Vent Settings")]
    [SerializeField] private int numOfAttemptsCompleted = 5;
    [SerializeField] private int numOfAttemptsAttempted;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

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