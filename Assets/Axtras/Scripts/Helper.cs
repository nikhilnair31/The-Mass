using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Helper : MonoBehaviour 
{
    public static Helper Instance { get; private set; }
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #region Vec Related
    public Vector3 GetDir(Transform interactable) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            return (hit.point - interactable.position).normalized;
        }
        else {
            return ray.direction;
        }
    }
    #endregion

    #region UI Related
    public bool DefineOnUI() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return true;
        }
        else {
            return false;
        }
    }
    #endregion

    #region Audio Related
    public void PlayRandAudio(AudioSource source, AudioClip[] clips) {
        var clip = clips[UnityEngine.Random.Range(0, clips.Length)];
        source.PlayOneShot(clip);
    }
    public void StartAudioLoop(AudioSource source, AudioClip clip, float delay) {
        source.clip = clip;
        source.loop = false;  // Disable built-in looping

        source.Play();
        
        DOVirtual.DelayedCall(
            clip.length + delay, 
            () => StartAudioLoop(source, clip, delay)
        );
    }
    #endregion

    #region Physics Related
    public void EnablePhysics(Rigidbody rb, bool active) {
        rb.useGravity = active;
        rb.isKinematic = !active;
    }
    #endregion

    #region Tween Related
    public void ScaleTween(Transform target, float waitTime = 1f) {
        float duration = 0.15f;
        if (target.localScale != Vector3.zero) {
            target.DOScale(Vector3.zero, duration).OnComplete(() => {
                target.DOScale(Vector3.one, duration)
                      .OnComplete(() => target.DOScale(Vector3.zero, duration).SetDelay(waitTime));
            });
        }
        else {
            target.DOScale(Vector3.one, duration)
                  .OnComplete(() => target.DOScale(Vector3.zero, duration).SetDelay(waitTime));
        }
    }
    #endregion
}