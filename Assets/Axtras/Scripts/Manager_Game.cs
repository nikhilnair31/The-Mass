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
            // case 0:
            //     Attempt1();
            //     break;
            case 1:
                Attempt1();
                break;
            case 2:
                Attempt2();
                break;
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
    private void Attempt1() {
        var source = phoneGO.GetComponent<AudioSource>();
        source.clip = ringtoneClip;
        source.loop = true;
        source.volume = 0f;
        source.Play();
        source.DOFade(1f, 5f);
    }
    private void Attempt2() {
        snowstormSource.DOFade(2f, 5f);
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