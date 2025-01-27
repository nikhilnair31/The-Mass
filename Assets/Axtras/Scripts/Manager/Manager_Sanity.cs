using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Rendering.Universal;

public class Manager_Sanity : MonoBehaviour 
{
    #region Vars
    public static Manager_Sanity Instance { get; private set; }

    [Header("Sanity Settings")]
    [SerializeField] private float sanityLostInTimeMax = 100f;
    [SerializeField] private float sanityLostInTime;
    [SerializeField] private float sanityRate = 1f;
    
    [Header("Proximity Settings")]
    [SerializeField] private float withinRangeOfMass = 2f;
    [SerializeField] private float sanityScaleRate = 2f;
    private Transform mass;

    [Header("Effects Settings")]
    [SerializeField] private CinemachineVolumeSettings postProcessVolume;
    private Vignette vignette;
    private LensDistortion lensDistortion;
    private SplitToning splitToning;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource sanityAudioSource;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        if (postProcessVolume.Profile.TryGet(out vignette)) {
            vignette.intensity.value = 0f;
        } else {
            Debug.LogError("Vignette not found.");
        }
        if (postProcessVolume.Profile.TryGet(out lensDistortion)) {
            lensDistortion.intensity.value = 0f;
        } else {
            Debug.LogError("LensDistortion not found.");
        }
        if (postProcessVolume.Profile.TryGet(out splitToning)) {
            splitToning.balance.value = -100f;
        } else {
            Debug.LogError("SplitToning not found.");
        }

        var massGO = GameObject.FindGameObjectWithTag("Mass");
        if (massGO != null) {
            mass = massGO.transform;
        }

        sanityAudioSource.volume = 0f;
    }

    private void Update() {
        if (mass == null) return;
        
        bool closeToMass  = Vector3.Distance(mass.position, transform.position) < withinRangeOfMass;
        float currentSanityRate = closeToMass ? sanityRate * sanityScaleRate : sanityRate;
        sanityLostInTime += currentSanityRate * Time.deltaTime;

        sanityLostInTime = Mathf.Clamp(sanityLostInTime, 0f, sanityLostInTimeMax);

        sanityAudioSource.volume = Mathf.Lerp(0f, 1f, sanityLostInTime / sanityLostInTimeMax);

        if (vignette != null) vignette.intensity.value = Mathf.Lerp(0f, 0.5f, sanityLostInTime / sanityLostInTimeMax);
        if (lensDistortion != null) lensDistortion.intensity.value = Mathf.Lerp(0f, 0.5f, sanityLostInTime / sanityLostInTimeMax);
        if (splitToning != null) splitToning.balance.value = Mathf.Lerp(-100f, 100f, sanityLostInTime / sanityLostInTimeMax);

        if (sanityLostInTime >= sanityLostInTimeMax) {
            Manager_UI.Instance.GameOver();
            sanityLostInTime = sanityLostInTimeMax;
        }
    }
    
    public void DisableSanity() {
        vignette.intensity.value = 0f;
        lensDistortion.intensity.value = 0f;
        splitToning.balance.value = -100f;

        enabled = false;
    }
}