using UnityEngine;

public class Manager_Game : MonoBehaviour 
{
    #region Vars
    public static Manager_Game Instance { get; private set; }

    [Header("Unlock Settings")]
    [SerializeField] private int numOfAttemptsCompleted = 5;
    [SerializeField] private int numOfAttemptsAttempted;

    [Header("Vent Settings")]
    [SerializeField] private Transform ventTranform;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddAttempt() {
        numOfAttemptsAttempted++;

        if (numOfAttemptsAttempted >= numOfAttemptsCompleted) {
            UnlockVent();
        }
    }

    private void UnlockVent() {
        Debug.Log($"UnlockVent");
        
        if (ventTranform.TryGetComponent(out Rigidbody rb)) {
            Helper.Instance.EnablePhysics(rb, true);
        }
    }
}