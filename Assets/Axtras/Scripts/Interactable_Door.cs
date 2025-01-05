using DG.Tweening;
using UnityEngine;

public class Interactable_Door : Controller_Interactables 
{
    #region Vars
    [Header("Door Settings")]
    [SerializeField] private Transform doorTransform;
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip[] doorAudioClips;
    [SerializeField] private float transitionTime = 1f;
    private bool isOpen = false;
    #endregion

    public override void InteractInteractable() {
        base.InteractInteractable();
        Debug.Log("Interactable_Switch InteractInteractable");

        Vector3 playerPosition = Camera.main.transform.position;
        Vector3 doorPosition = doorTransform.position;

        Vector3 doorForward = doorTransform.forward;
        Vector3 playerToDoor = (playerPosition - doorPosition).normalized;

        float dotProduct = Vector3.Dot(doorForward, playerToDoor);

        float currentYRotation = doorTransform.localEulerAngles.y;
        float targetYRotation = isOpen ? currentYRotation - 90f : currentYRotation + 90f;

        if (dotProduct < 0) {
            targetYRotation = isOpen ? currentYRotation + 90f : currentYRotation - 90f;
        }

        doorTransform.DORotate(new Vector3(0f, targetYRotation, 0f), transitionTime);

        PlayRandAudio(doorAudioSource, doorAudioClips);

        isOpen = !isOpen;
    }
}