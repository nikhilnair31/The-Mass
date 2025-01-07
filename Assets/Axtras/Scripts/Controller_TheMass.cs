using UnityEngine;
using DG.Tweening;

public class Controller_TheMass : MonoBehaviour 
{
    #region Vars
    public static Controller_TheMass Instance { get; private set; }

    [SerializeField] private Vector3 growScale;
    private Vector3 currScale;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        currScale = transform.localScale;
    }

    public void GrowTheMass() {
        currScale = transform.localScale + growScale;
        transform.DOScale(currScale, 1f);
    }
}