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

    // [Header("Game Settings")]
    // [SerializeField] private GameObject snowstorm;
    // [SerializeField] private GameObject phone;
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

    private void Start() {
        numOfAttemptsAttempted = 0;
    }

    private void UpdateByAttempt() {
        switch (numOfAttemptsAttempted) {
            // case 0:
            //     snowstorm.Run(false);
            //     phone.Run(false);
            //     break;
            // case 1:
            //     snowstorm.Run(true);
            //     phone.Run(false);
            //     break;
            // case 2:
            //     snowstorm.Run(true);
            //     phone.Run(true);
            //     break;
            // case 3:
            //     snowstorm.Run(false);
            //     phone.Run(false);
            //     break;
            // case 4:
            //     snowstorm.Run(false);
            //     phone.Run(false);
            //     break;
            // case 5:
            //     snowstorm.Run(false);
            //     phone.Run(false);
            //     break;
            default:
                break;
        }
    }

    public void AddAttempt() {
        numOfAttemptsAttempted++;

        UpdateByAttempt();

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