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

    #region Tween Related
    public void ScaleTween(Transform target, float waitTime = 1f) {
        float duration = 0.5f;
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