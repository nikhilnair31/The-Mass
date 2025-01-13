using UnityEngine;

public class Manager_Thoughts : MonoBehaviour 
{
    #region Vars
    public static Manager_Thoughts Instance { get; private set; }
    
    [Header("Thoughts Settings")]
    [SerializeField] private AudioSource thoughtsAudioSource;
    [SerializeField] private AudioClip[] typingClips;
    [SerializeField] private float showTypingSpeed = 0.05f;
    [SerializeField] private float hideTypingSpeed = 0.02f;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateThoughtText(string showTextStr) {

        StopAllCoroutines();
        StartCoroutine(Manager_UI.Instance.ShowTextWithSound(
            thoughtsAudioSource,
            typingClips,
            showTypingSpeed,
            showTextStr
        ));
    }
    public void ClearThoughtText() {
        StopAllCoroutines();
        StartCoroutine(Manager_UI.Instance.ClearText(
            hideTypingSpeed
        ));
    }    
}