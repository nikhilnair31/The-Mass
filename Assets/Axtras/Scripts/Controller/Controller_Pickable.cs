using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Controller_Pickable : Controller_Interactables 
{
    #region Vars
    [Header("Pickable Settings")]
    [SerializeField] private bool isPickable = true;
    [SerializeField] internal bool wasHeld = false;
    [SerializeField] private Vector3 heldPos = Vector3.zero;
    [SerializeField] private Vector3 heldRotEuler = Vector3.zero;
    #endregion

    public virtual void Start() {
        rgb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    
    public virtual void PickInteractable() {
        var rb = transform.GetComponent<Rigidbody>();
        Helper.Instance.EnablePhysics(rb, false);

        transform.SetParent(playerController.holdAtTransform);

        var sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOLocalMove(heldPos, 0.5f))
            .Join(transform.DOLocalRotate(heldRotEuler, 0.5f))
            .OnComplete(() => {
                playerController.heldInteractable = transform;
            });
    }
    public virtual void DropInteractable() {
        var rb = transform.GetComponent<Rigidbody>();
        Helper.Instance.EnablePhysics(rb, true);
        playerController.heldInteractable.SetParent(null);
        playerController.heldInteractable = null;
    }
    
    public void SetIsPickable(bool active) {
        isPickable = active;
    }
    public void SetWasPicked(bool picked) {
        wasHeld = picked;
    }
    public bool ReturnPickableBool() {
        return isPickable;
    }
}