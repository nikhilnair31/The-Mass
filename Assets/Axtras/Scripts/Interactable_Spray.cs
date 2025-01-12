using UnityEngine;

public class Interactable_Spray : Controller_Pickable 
{
    #region Vars
    [Header("Spray Settings")]
    [SerializeField] private ParticleSystem sprayPS;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip sprayClip;
    #endregion

    public override void Start() {
        base.Start();

        var collisionModule = sprayPS.collision;
        collisionModule.enabled = true;
    }

    public void StartSpray() {
        sprayPS.Play();
        audioSource.clip = sprayClip;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void StopSpray() {
        sprayPS.Stop();
        audioSource.Stop();
    }
}