using UnityEngine;

public class Interactable_Spray : Controller_Pickable 
{
    #region Vars
    private Interactable_SprayParticles sprayparticles;

    [Header("Spray Settings")]
    [SerializeField] private ParticleSystem sprayPS;
    [SerializeField] private bool isSprayingMass = false;
    [SerializeField] private float requiredSprayTime = 5f;
    [SerializeField] private float resetTime = 0.1f;
    private float sprayTimer = 0f;
    private float lastCollisionTime = 0f;

    [Header("Fire Settings")]
    [SerializeField] private bool isFireSpray = false;
    [SerializeField] private Light fireLight;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip sprayClip;
    #endregion

    public override void Start() {
        base.Start();

        sprayparticles = sprayPS.GetComponent<Interactable_SprayParticles>();
        
        var collisionModule = sprayPS.collision;
        collisionModule.enabled = true;
    }

    private void Update() {
        if (isSprayingMass) {
            if (Time.time - lastCollisionTime > resetTime) {
                isSprayingMass = false;
                sprayTimer = 0f;
                return;
            }

            sprayTimer += Time.deltaTime;

            if (sprayTimer >= requiredSprayTime) {
                if (isFireSpray) {
                    Controller_TheMass.Instance.GotHit("Spray_Fire");
                }
                else if (!isFireSpray) {
                    Controller_TheMass.Instance.GotHit("Spray_Gas");
                }
            }
        }
    }

    public void StartSpray() {
        sprayPS.Play();
        audioSource.clip = sprayClip;
        audioSource.loop = true;
        audioSource.Play();

        if (isFireSpray) {
            fireLight.enabled = true;
        }
    }

    public void StopSpray() {
        isSprayingMass = false;
        sprayTimer = 0f;
        sprayPS.Stop();
        audioSource.Stop();
        
        if (isFireSpray) {
            fireLight.enabled = false;
        }
    }

    public void SetSprayingMass(bool active) {
        isSprayingMass = active;
        if (active) {
            lastCollisionTime = Time.time;
        }
    }
}