using UnityEngine;
using UnityEngine.Rendering;
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
    [SerializeField] private Volume postProcessVolume;
    private ChromaticAberration chromaticAberration;

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
        if (postProcessVolume.profile.TryGet(out chromaticAberration)) {
            chromaticAberration.intensity.value = 0f;
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

        if (chromaticAberration != null) {
            chromaticAberration.intensity.value = Mathf.Lerp(0f, 1f, sanityLostInTime / sanityLostInTimeMax);
        }

        if (sanityLostInTime >= sanityLostInTimeMax) {
            Manager_UI.Instance.GameOver();
            sanityLostInTime = sanityLostInTimeMax;
        }
    }
}