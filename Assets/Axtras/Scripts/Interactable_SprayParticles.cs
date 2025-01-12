using UnityEngine;

public class Interactable_SprayParticles : MonoBehaviour 
{
    #region Vars
    [Header("Spray Particles Settings")]
    [SerializeField] private bool hasHitMass;
    #endregion

    private void OnParticleCollision(GameObject other) {
        // Debug.Log($"Particle from {transform.name} collided with {other.name}");

        if (!hasHitMass && other.transform.CompareTag("Mass")) {
            hasHitMass = true;
            Controller_TheMass.Instance.GotHit();
        }
    }    
}