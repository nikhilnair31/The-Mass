using UnityEngine;

public class Manager_Game : MonoBehaviour 
{
    #region Vars
    public static Manager_Game Instance { get; private set; }

    [Header("Unlock Settings")]
    [SerializeField] private int numOfAttemptsCompleted = 5;
    [SerializeField] private int numOfAttemptsAttempted;
    [SerializeField] private bool addAttempt;

    [Header("Vent Settings")]
    [SerializeField] private Transform ventTranform;
    #endregion

    #if UNITY_EDITOR
    private void OnValidate() {
        if (addAttempt) {
            AddAttempt();
            addAttempt = false;
        }
    }
    #endif

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
            var throwable = ventTranform.GetComponent<Controller_Interactables>();
            throwable.SetInteractionText("pick up?");

            Helper.Instance.EnablePhysics(rb, true);
        }
    }
}