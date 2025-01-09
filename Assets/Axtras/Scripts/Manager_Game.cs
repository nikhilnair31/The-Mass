using DG.Tweening;
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
    [SerializeField] private GameObject ventColliderGO;

    [Header("Attempt 1 Settings")]
    [SerializeField] private GameObject phoneGO;
    [SerializeField] private AudioClip ringtoneClip;

    [Header("Attempt 2 Settings")]
    [SerializeField] private GameObject snowstormGO;
    [SerializeField] private AudioSource snowstormSource;
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
            case 1:
                Attempt1();
                break;
            case 2:
                Attempt2();
                break;
            case 3:
                Attempt3();
                break;
            case 4:
                Attempt4();
                break;
            case 5:
                Attempt5();
                break;
            default:
                break;
        }
    }
    private void Attempt1() {
        // Call comes on broken phone
        var source = phoneGO.GetComponent<AudioSource>();
        source.clip = ringtoneClip;
        source.loop = true;
        source.volume = 0f;
        source.Play();
        source.DOFade(1f, 5f);
    }
    private void Attempt2() {
        // Snowstorm gets stronger
        snowstormSource.DOFade(2f, 5f);
        // Water starts dripping from taps
    }
    private void Attempt3() {
        // Clocks stop
        // Water from taps flows much faster
    }
    private void Attempt4() {
        // Visible breath as it gets colder 
        // Locked door room opens
    }
    private void Attempt5() {
        // Some objects move from their previous place
        // New small objects appear 
    }

    public bool AllAttemptsCompleted() {
        return numOfAttemptsAttempted == numOfAttemptsCompleted;
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

        ventColliderGO.SetActive(true);
    }
}