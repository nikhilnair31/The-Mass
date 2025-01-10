using DG.Tweening;
using UnityEngine;

public class Manager_Game : MonoBehaviour 
{
    #region Vars
    public static Manager_Game Instance { get; private set; }

    [Header("Attempts Settings")]
    [SerializeField] private int numOfAttemptsCompleted = 5;
    [SerializeField] private int numOfAttemptsAttempted;
    [SerializeField] private bool addAttempt;

    [Header("Vent Settings")]
    [SerializeField] private Transform ventTranform;
    [SerializeField] private GameObject ventColliderGO;
    [SerializeField] private ParticleSystem ventExitSteamPS;

    [Header("Snowstorm Settings")]
    [SerializeField] private ParticleSystem snowstormPS;
    [SerializeField] private AudioSource snowstormSource;

    [Header("Kitchen Tap Settings")]
    [SerializeField] private AudioSource kitchenTapWaterSource;
    [SerializeField] private ParticleSystem kitchenTapWaterWeakPS;
    [SerializeField] private ParticleSystem kitchenTapWaterStrongPS;

    [Header("Toilet Tap Settings")]
    [SerializeField] private AudioSource toiletTapWaterSource;
    [SerializeField] private ParticleSystem toiletTapWaterWeakPS;
    [SerializeField] private ParticleSystem toiletTapWaterStrongPS;

    [Header("Cold Breath Settings")]
    [SerializeField] private AudioSource coldPlayerBreathSource;
    [SerializeField] private ParticleSystem coldPlayerBreathPS;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip weakTapWaterClip;
    [SerializeField] private AudioClip strongTapWaterClip;
    [SerializeField] private AudioClip coldBreathClip;
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

    public void AddAttempt() {
        numOfAttemptsAttempted++;

        UpdateByAttempt();

        if (numOfAttemptsAttempted >= numOfAttemptsCompleted) {
            UnlockVent();
        }
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
                Attempt0();
                break;
        }
    }
    private void Attempt0() {
        ventExitSteamPS.Stop();
    }
    private void Attempt1() {
        // EMP effects that make the phone ring adn lights get brighter
        
        // Call audio
        Controller_Phone.Instance.MakeCall();
    }
    private void Attempt2() {
        // Weather and tap water effects

        // Start snowstorm
        snowstormPS.Play();
        // Increase snowstorm loudness
        DOVirtual.DelayedCall(
            3f, 
            () => snowstormSource.DOFade(2f, 5f)
        );

        // Water starts dripping from taps
        kitchenTapWaterWeakPS.Play();
        toiletTapWaterWeakPS.Play();
        Helper.Instance.StartAudioLoop(kitchenTapWaterSource, weakTapWaterClip, 0f);
        Helper.Instance.StartAudioLoop(toiletTapWaterSource, weakTapWaterClip, 0f);
    }
    private void Attempt3() {
        // Clocks speed up rapidly
        Controller_Clock.Instance.SetTimeMul(20f);

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
        // Start playing steam to vent exit to draw attention
        DOVirtual.DelayedCall(
            3f, 
            () => ventExitSteamPS.Play()
        );

        // Some objects move from their previous place
        
        // New small objects appear 
    }

    public bool GetIfAllAttemptsCompleted() {
        return numOfAttemptsAttempted == numOfAttemptsCompleted;
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