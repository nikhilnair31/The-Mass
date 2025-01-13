using UnityEngine;

public class Interactable_SprayParticles : MonoBehaviour 
{
    #region Vars
    [Header("Spray Particles Settings")]
    [SerializeField] private Interactable_Spray sprayInteractable;
    #endregion

    private void OnParticleCollision(GameObject other) {
        if (other.transform.CompareTag("Mass")) {
            sprayInteractable.SetSprayingMass(true);
        }
    }
}