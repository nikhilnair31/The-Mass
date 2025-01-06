using DG.Tweening;
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
    #endregion

    public override void InteractInteractable(Transform currentInteractable) {
        base.InteractInteractable(currentInteractable);

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

            PlayRandAudio(audioSource, audioClips);

            isOpen = !isOpen;
        }
        else {
            float startAngleY = 0f;
            transform.DOShakeRotation(shakeTime,shakeVec, 10, 90f)
                .OnStart(() => startAngleY = transform.localEulerAngles.y)
                .OnComplete(() => transform.rotation = Quaternion.Euler(0f, startAngleY, 0f));

            PlayRandAudio(audioSource, audioClips);
        }
    }
}