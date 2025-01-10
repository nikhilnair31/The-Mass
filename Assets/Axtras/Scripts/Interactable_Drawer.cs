using DG.Tweening;
using UnityEngine;

public class Interactable_Drawer : Controller_Interactables 
{
    #region Vars
    [Header("Drawer Settings")]
    [SerializeField] private Vector3 openPos;
    private bool isOpen = false;

    [Header("Lock Settings")]
    [SerializeField] private Vector3 shakeVec;
    [SerializeField] private float shakeTime = 0.2f;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private bool drawerIsLocked = false;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] opencloseClips;
    [SerializeField] private AudioClip[] lockedClips;
    #endregion

    public override void Start() {
        base.Start();

        rgb.mass = 1;
        rgb.isKinematic = true;
        rgb.useGravity = false;

        audioSource.spatialBlend = 1;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void ControlOpenCloseDrawer() {
        if (!drawerIsLocked) {
            Vector3 currPos = transform.localPosition;
            Vector3 targetPos = isOpen ? currPos - openPos : currPos + openPos;

            transform
                .DOLocalMove(targetPos, transitionTime)
                .SetEase(Ease.InOutSine);

            Helper.Instance.PlayRandAudio(audioSource, opencloseClips);

            isOpen = !isOpen;
        }
        else {
            float startAngleY = 0f;
            transform.DOShakeRotation(shakeTime,shakeVec, 10, 90f)
                .OnStart(() => {
                    startAngleY = transform.localEulerAngles.y;
                })
                .OnComplete(() => {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, startAngleY, transform.localEulerAngles.z);
                });
            
            showThisText = "it's locked";

            Helper.Instance.PlayRandAudio(audioSource, lockedClips);
        }
    }

    public void SetIsDrawerLocked(bool active) {
        drawerIsLocked = active;
    }
    public bool GetIsDrawerLocked() {
        return drawerIsLocked;
    }
}