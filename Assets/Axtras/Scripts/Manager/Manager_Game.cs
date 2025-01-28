using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Manager_Game : MonoBehaviour 
{
    #region Vars
    public static Manager_Game Instance { get; private set; }

    [Header("Attempts Settings")]
    [SerializeField] private int maxAttempts = 5;
    [SerializeField] private int currentAttempts;
    [SerializeField] private bool addAttempt;
    [SerializeField] private List<Data_Approach> approaches = new() {
        new Data_Approach { Name = "Throwable_Regular", IsUsed = false, ThoughtText = "i should just throw something at it" },
        new Data_Approach { Name = "Throwable_Breakable", IsUsed = false, ThoughtText = "something that would shatter" },
        new Data_Approach { Name = "Pokable", IsUsed = false, ThoughtText = "something long..." },
        new Data_Approach { Name = "Spray_Gas", IsUsed = false, ThoughtText = "maybe it's just some bug?" },
        new Data_Approach { Name = "Spray_Fire", IsUsed = false, ThoughtText = "i have to burn that ... that <b><color=red>MASS</color></b> away" }
    };

    [Header("Vent Settings")]
    [SerializeField] private Transform ventTranform;
    [SerializeField] private GameObject ventEntryColliderGO;
    [SerializeField] private ParticleSystem ventExitSteamPS;

    [Header("Door Settings")]
    [SerializeField] private Interactable_Door roomateDoorInteractable;

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

    // #if UNITY_EDITOR
    // private void OnValidate() {
    //     if (addAttempt) {
    //         addAttempt = false;
    //         foreach (var approach in approaches) {
    //             if (!approach.IsUsed) {
    //                 AddAttempt(approach.Name);
    //             }
    //         }
    //     }
    // }
    // #endif

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        UpdateByAttempt();
    }
 
    public bool AddAttempt(string approachName) {
        if (currentAttempts >= maxAttempts) {
            Debug.Log("All attempts completed!");
            return false;
        }

        var approach = approaches.Find(a => a.Name == approachName);
        if (approach == null) {
            Debug.LogError("Invalid approach type!");
            return false;
        }

        if (approach.IsUsed) {
            Debug.LogWarning($"Approach '{approachName}' has already been used!");
            return false;
        }

        currentAttempts++;
        approach.IsUsed = true;
        Debug.Log($"Attempt unlocked using '{approachName}'. Total attempts: {currentAttempts}/{maxAttempts}");

        UpdateByAttempt();

        return true;
    }

    private void UpdateByAttempt() {
        switch (currentAttempts) {
            case 1:
                Attempt1();
                ShowNextAttemptThought(NextAttemptThought());
                break;
            case 2:
                Attempt2();
                ShowNextAttemptThought(NextAttemptThought());
                break;
            case 3:
                Attempt3();
                ShowNextAttemptThought(NextAttemptThought());
                break;
            case 4:
                Attempt4();
                ShowNextAttemptThought(NextAttemptThought());
                break;
            case 5:
                Attempt5();
                ShowNextAttemptThought(NextAttemptThought());
                break;
            default:
                Attempt0();
                break;
        }
    }
    private void Attempt0() {
        ventExitSteamPS.Stop();
        
        if (ventTranform.TryGetComponent(out Rigidbody rb)) {
            Helper.Instance.EnablePhysics(rb, false);
        }

        ventEntryColliderGO.SetActive(false);
    }
    private void Attempt1() {
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
        // Visible breath as it gets colder 
        coldPlayerBreathPS.Play();
        Helper.Instance.StartAudioLoop(coldPlayerBreathSource, coldBreathClip, 1f);

        // Clocks speed up rapidly
        Controller_Clock.Instance.SetTimeMul(200f);

        // Water from taps flows much faster
        kitchenTapWaterStrongPS.Play();
        toiletTapWaterStrongPS.Play();
        Helper.Instance.StartAudioLoop(kitchenTapWaterSource, strongTapWaterClip, 0f);
        Helper.Instance.StartAudioLoop(toiletTapWaterSource, strongTapWaterClip, 0f);
    }
    private void Attempt4() {
        // Locked doors open
        var allDoors = FindObjectsByType<Interactable_Door>(FindObjectsSortMode.None);
        foreach (var door in allDoors) {
            if (door.GetIsDoorLocked()) {
                door.SetIsDoorLocked(false);
                door.SetInteractionText("open/close door?");
                door.ControlOpenCloseDoor();
            }
        }

        // Locked drawers open
        var allDrawers = FindObjectsByType<Interactable_Drawer>(FindObjectsSortMode.None);
        foreach (var drawer in allDrawers) {
            if (drawer.GetIsDrawerLocked()) {
                drawer.SetIsDrawerLocked(false);
                drawer.SetInteractionText("open/close drawer?");
                drawer.ControlOpenCloseDrawer();
            }
        }
    }
    public void Attempt5() {
        // Unlock the vent cover
        if (ventTranform.TryGetComponent(out Rigidbody rb)) {
            var throwable = ventTranform.GetComponent<Controller_Pickable>();
            throwable.SetInteractionText("pick up?");
            throwable.SetIsPickable(true);

            Helper.Instance.EnablePhysics(rb, true);
        }
        ventEntryColliderGO.SetActive(true);

        // Start playing steam to vent exit to draw attention
        DOVirtual.DelayedCall(
            3f, 
            () => ventExitSteamPS.Play()
        );

        // Some objects move from their previous place
        
        // New small objects appear 
    }

    private string NextAttemptThought() {
        string nextAttemptThought = "WAIT! I can get closer through the vent!";

        foreach (var approach in approaches) {
            Debug.Log($"approach: {approach}");
            if (!approach.IsUsed) {
                return approach.ThoughtText;
            }
        }

        return nextAttemptThought;
    }
    private void ShowNextAttemptThought(string nextAttemptThought) {    
        DOVirtual.DelayedCall(
            3f, 
            () => {
                Manager_Thoughts.Instance.ShowText(
                    nextAttemptThought, 
                    5f,
                    Manager_Thoughts.TextPriority.Player
                );
            }
        );
    }
}