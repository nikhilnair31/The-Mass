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

    [Header("Audio Settings")]
    [SerializeField] private AudioSource impactAudioSource;
    [SerializeField] private AudioClip[] impactClips;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        GrowTheMass();
    }

    public void GotHit(string approach) {
        Debug.Log($"GotHit by {approach}");
        
        Manager_Game.Instance.AddAttempt(approach);
        Helper.Instance.PlayRandAudio(impactAudioSource, impactClips);
    }
    
    private void GrowTheMass() {
        var prevScale = transform.localScale;
        var targetScale = transform.localScale + growScale;
        var growthSequence = DOTween.Sequence();
        growthSequence
            .Join(transform.DOShakeRotation(shakeForTime, shakeScale, 3, 90f))
            .Join(transform.DOScale(transform.localScale + growScale, growInTime))
            .AppendInterval(growAfterTime)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }
}