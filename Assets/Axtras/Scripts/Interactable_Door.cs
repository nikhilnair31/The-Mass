using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Interactable_Door : Controller_Interactables 
{
    #region Vars
    [Header("Door Settings")]
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private float shakeTime = 0.2f;
    [SerializeField] private Vector3 shakeVec;
    [SerializeField] private bool doorIsLocked = false;
    private bool isOpen = false;

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
    }

    public void ControlOpenCloseDoor() {
        if (!doorIsLocked) {
            Vector3 playerPosition = Camera.main.transform.position;
            Vector3 doorPosition = transform.position;

            Vector3 doorForward = transform.forward;
            Vector3 playerToDoor = (playerPosition - doorPosition).normalized;

            float dotProduct = Vector3.Dot(doorForward, playerToDoor);

            float currentYRotation = transform.localEulerAngles.y;
            float targetYRotation = isOpen ? currentYRotation - 90f : currentYRotation + 90f;

            if (dotProduct < 0) {
                targetYRotation = isOpen ? currentYRotation + 90f : currentYRotation - 90f;
            }

            transform.DORotate(new Vector3(0f, targetYRotation, 0f), transitionTime);

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