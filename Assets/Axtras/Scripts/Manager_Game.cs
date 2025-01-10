using System.Xml.Linq;
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
    [SerializeField] private AudioSource phoneSource;
    [SerializeField] private AudioClip ringtoneClip;

    [Header("Attempt 2 Settings")]
    [SerializeField] private ParticleSystem snowstormPS;
    [SerializeField] private AudioSource snowstormSource;
    [SerializeField] private AudioSource kitchenTapWaterSource;
    [SerializeField] private AudioSource toiletTapWaterSource;
    [SerializeField] private AudioClip weakTapWaterClip;
    [SerializeField] private ParticleSystem kitchenTapWaterWeakPS;
    [SerializeField] private ParticleSystem toiletTapWaterWeakPS;

    [Header("Attempt 3 Settings")]
    [SerializeField] private AudioClip strongTapWaterClip;
    [SerializeField] private ParticleSystem kitchenTapWaterStrongPS;
    [SerializeField] private ParticleSystem toiletTapWaterStrongPS;

    [Header("Attempt 4 Settings")]
    [SerializeField] private AudioSource coldPlayerBreathSource;
    [SerializeField] private AudioClip coldBreathClip;
    [SerializeField] private ParticleSystem coldPlayerBreathPS;

    [Header("Attempt 5 Settings")]
    [SerializeField] private float idk;
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
        phoneSource.clip = ringtoneClip;
        phoneSource.loop = true;
        phoneSource.volume = 0f;
        phoneSource.DOFade(1f, 5f);
        Helper.Instance.StartAudioLoop(phoneSource, ringtoneClip, 1f);
    }
    private void Attempt2() {
        // Snowstorm gets stronger
        snowstormPS.Play();
        snowstormSource.DOFade(2f, 5f);
        // Water starts dripping from taps
        kitchenTapWaterWeakPS.Play();
        toiletTapWaterWeakPS.Play();
        Helper.Instance.StartAudioLoop(kitchenTapWaterSource, weakTapWaterClip, 0f);
        Helper.Instance.StartAudioLoop(toiletTapWaterSource, weakTapWaterClip, 0f);
    }
    private void Attempt3() {
        // Clocks stop
        // Water from taps flows much faster
        kitchenTapWaterStrongPS.Play();
        toiletTapWaterStrongPS.Play();
        Helper.Instance.StartAudioLoop(kitchenTapWaterSource, strongTapWaterClip, 0f);
        Helper.Instance.StartAudioLoop(toiletTapWaterSource, strongTapWaterClip, 0f);
    }
    private void Attempt4() {
        // Visible breath as it gets colder 
        coldPlayerBreathPS.Play();
        Helper.Instance.StartAudioLoop(coldPlayerBreathSource, coldBreathClip, 1f);
        // Locked door room opens
        var allDoors = FindObjectsByType<Interactable_Door>(FindObjectsSortMode.None);
        foreach (var door in allDoors) {
            if (door.GetIsDoorLocked()) {
                door.SetIsDoorLocked(false);
            }
        }
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