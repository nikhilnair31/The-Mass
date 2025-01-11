using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Controller_Pickable : Controller_Interactables 
{
    #region Vars
    [Header("Pickable Settings")]
    [SerializeField] private bool isPickable = true;
    [SerializeField] internal bool wasHeld = false;
    [SerializeField] private Vector3 heldRotEuler = Vector3.zero;
    #endregion
    
    public virtual void PickInteractable() {
        var rb = transform.GetComponent<Rigidbody>();
        Helper.Instance.EnablePhysics(rb, false);

        transform.SetParent(playerController.holdAtTransform);
        transform.localEulerAngles = Vector3.zero;

        transform
            .DOLocalMove(
                Vector3.zero, 
                0.5f
            )
            .OnComplete(() => {
                playerController.heldInteractable = transform;
            });
        
        transform
            .DOLocalRotate(
                heldRotEuler, 
                0.5f
            );
    }
    public virtual void DropInteractable() {
        var rb = transform.GetComponent<Rigidbody>();
        Helper.Instance.EnablePhysics(rb, true);
        playerController.heldInteractable.SetParent(null);
        playerController.heldInteractable = null;
    }
    
    public void SetWasPicked(bool picked) {
        wasHeld = picked;
    }
    public bool ReturnPickableBool() {
        return isPickable;
    }
}