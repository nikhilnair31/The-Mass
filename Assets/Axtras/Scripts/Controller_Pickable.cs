using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Controller_Pickable : Controller_Interactables 
{
    #region Vars
    [Header("Pickable Settings")]
    [SerializeField] private bool isPickable = true;
    #endregion
    
    public virtual void PickInteractable() {
        if (!transform.TryGetComponent(out Rigidbody rb)) {
            rb = transform.AddComponent<Rigidbody>();
        }

        Helper.Instance.EnablePhysics(rb, false);

        transform.SetParent(playerController.holdAtTransform);

        transform.DOLocalMove(Vector3.zero, 0.5f)
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
    
    public bool ReturnPickableBool() {
        return isPickable;
    }
}