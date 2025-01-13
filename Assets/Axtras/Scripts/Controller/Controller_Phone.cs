using DG.Tweening;
using UnityEngine;

public class Controller_Phone : MonoBehaviour 
{
    #region Vars
    public static Controller_Phone Instance { get; private set; }

    [Header("Phone Settings")]
    [SerializeField] private AudioSource phoneSource;
    [SerializeField] private float fullVolume = 2f;

    [Header("Visuals Settings")]
    [SerializeField] private MeshRenderer meshRend;
    [SerializeField] private Material screenMat;

    [Header("Call Settings")]
    [SerializeField] private AudioClip ringtoneClip;
    [SerializeField] private float startDelay = 3f;
    [SerializeField] private float ringtoneGapDelay = 2f;
    private bool callIsOn = false;

    [Header("Notification Settings")]
    [SerializeField] private AudioClip notifClip;
    [SerializeField] private float notifRandDelay = 15f;

    [Header("Vinration Settings")]
    [SerializeField] private Vector3 shakeVec;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        var screenMatList = meshRend.materials;
        foreach (var mat in screenMatList) {
            if (mat.HasProperty("_EmissionColor")) {
                screenMat = mat;
            }
        }

        RandNotification();    
    }
    
    private void RandNotification() {
        if (!callIsOn) {
            Vector3 initRot = new();
            float audioDuration = notifClip.length;
            Sequence notificationSequence = DOTween.Sequence();
            notificationSequence
                .OnStart(() => {
                    // Store the initial rotation
                    initRot = transform.localEulerAngles;

                    // Enable material emission
                    screenMat.EnableKeyword("_EMISSION");

                    // Start playing the audio
                    phoneSource.clip = notifClip;
                    phoneSource.loop = false;
                    phoneSource.volume = fullVolume;
                    phoneSource.Play();

                    // Start shaking the object
                    transform.DOShakeRotation(audioDuration, shakeVec, 10, 90f);
                })
                // Step 2: Delay for the duration of the audio
                .AppendInterval(audioDuration)
                .OnComplete(() => {
                    // Reset the rotation and disable material emission
                    transform.localEulerAngles = initRot;

                    // Enable material emission
                    screenMat.DisableKeyword("_EMISSION");

                    // Repeat the notification with a random delay
                    DOVirtual.DelayedCall(
                        Random.Range(notifRandDelay, notifRandDelay * 2),
                        RandNotification
                    );
                });
        }
    }

    public void MakeCall() {
        callIsOn = true;
        Vector3 initRot = new();
        float audioDuration = ringtoneClip.length;
        Sequence callSequence = DOTween.Sequence();
        callSequence
            .OnStart(() => {
                // Store the initial rotation
                initRot = transform.localEulerAngles;

                // Set audio attributes
                phoneSource.clip = ringtoneClip;
                phoneSource.loop = false;
                phoneSource.volume = fullVolume;
                
                DOVirtual.DelayedCall(
                    startDelay, 
                    () => {
                        // Enable material emission
                        screenMat.EnableKeyword("_EMISSION");
                        // Play the audio after some delay
                        phoneSource.Play();
                        // Start shaking the object
                        transform.DOShakeRotation(audioDuration, shakeVec, 10, 90f);
                    }
                );
            })
            // Step 2: Delay for the duration of the audio
            .AppendInterval(
                audioDuration
            )
            .OnComplete(() => {
                // Reset the rotation and disable material emission
                transform.localEulerAngles = initRot;

                // Repeat the notification with a random delay
                Helper.Instance.StartAudioLoop(phoneSource, ringtoneClip, ringtoneGapDelay);
            });
    }
}