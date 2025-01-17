using UnityEngine;
using DG.Tweening;

public class Controller_TheMass : MonoBehaviour 
{
    #region Vars
    public static Controller_TheMass Instance { get; private set; }

    [Header("Growing Settings")]
    [SerializeField] private Vector3 growScale;
    [SerializeField] private Vector3 shakeScale;
    [SerializeField] private float shakeForTime;
    [SerializeField] private float growAfterTime;
    [SerializeField] private float growInTime;
    private float scaleIncrease = 1f;
    private Vector3 originalScale;
    private Sequence scaleSequence;
    private Vector3 targetScale;
    private Vector3 prevScale;

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

        GrowTheMass();
    }

    public void GotHit(string approach) {
        Debug.Log($"GotHit by {approach}");
        
        Manager_Game.Instance.AddAttempt(approach);
        Helper.Instance.PlayRandAudio(audioSource, impactClips);
    }
    
    private void GrowTheMass() {
        prevScale = transform.localScale;
        targetScale = transform.localScale + growScale;
        Sequence growthSequence = DOTween.Sequence();
        growthSequence
            .Join(transform.DOShakeRotation(shakeForTime, shakeScale, 3, 90f))
            .Join(transform.DOScale(transform.localScale + growScale, growInTime))
            .AppendInterval(growAfterTime)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }
}