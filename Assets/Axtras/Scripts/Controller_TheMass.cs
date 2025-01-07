using UnityEngine;
using DG.Tweening;

public class Controller_TheMass : MonoBehaviour 
{
    #region Vars
    public static Controller_TheMass Instance { get; private set; }

    [Header("Growing Settings")]
    [SerializeField] private Vector3 growScale;
    private Vector3 currScale;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] impactClips;
    private AudioSource audioSource;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        currScale = transform.localScale;
    }

    public void GotHit() {
        Manager_Game.Instance.AddAttempt();
        Helper.Instance.PlayRandAudio(audioSource, impactClips);
    }
    public void GrowTheMass() {
        currScale = transform.localScale + growScale;
        transform.DOScale(currScale, 1f);
    }
}