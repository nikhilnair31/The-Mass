using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Interactable_Door : Controller_Interactables 
{
    #region Vars
    [Header("Door Settings")]
    [SerializeField] private bool dependsOnPlayerLoc = true;
    [SerializeField] private Vector3 rotVec;
    [SerializeField] private bool isOpen = false;

    [Header("Lock Settings")]
    [SerializeField] private Vector3 shakeVec;
    [SerializeField] private float shakeTime = 0.2f;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private bool doorIsLocked = false;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] opencloseClips;
    [SerializeField] private AudioClip[] lockedClips;
    #endregion

    public override void Start() {
        base.Start();

        rgb.mass = 20;
        rgb.isKinematic = true;
        rgb.useGravity = false;

        audioSource.spatialBlend = 1;
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        if (isOpen) {
            transform.localEulerAngles = transform.localEulerAngles + rotVec;
        }
    }

    public void ControlOpenCloseDoor() {
        Debug.Log($"ControlOpenCloseDoor");
        
        if (!doorIsLocked) {
            Vector3 targetRotation = Vector3.zero;
            Vector3 currentRotation = transform.localEulerAngles;
            
            if (dependsOnPlayerLoc) {
                Vector3 playerPosition = Camera.main.transform.position;
                Vector3 doorPosition = transform.position;

                Vector3 doorForward = transform.forward;
                Vector3 playerToDoor = (playerPosition - doorPosition).normalized;

                float dotProduct = Vector3.Dot(doorForward, playerToDoor);

                if (dotProduct > 0) {
                    targetRotation = isOpen ? currentRotation - rotVec : currentRotation + rotVec;
                } 
                else {
                    targetRotation = isOpen ? currentRotation + rotVec : currentRotation - rotVec;
                }
            }
            else {
                targetRotation = isOpen ? Vector3.zero : rotVec;
            }

            transform
                .DOLocalRotate(targetRotation, transitionTime)
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

    public void SetIsDoorLocked(bool active) {
        doorIsLocked = active;
    }
    public bool GetIsDoorLocked() {
        return doorIsLocked;
    }
}