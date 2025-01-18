using System.Collections;
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

    public IEnumerator ShowTextSequence(string text, float showTime) {
        // Debug.Log($"ShowTextSequence");

        Manager_UI.Instance.SetShowText(text);
        // StopAllCoroutines();
        // StartCoroutine(Manager_UI.Instance.ShowTextWithSound(
        //     thoughtsAudioSource,
        //     typingClips,
        //     showTypingSpeed,
        //     text
        // ));

        if (showTime >= 0) {
            yield return new WaitForSeconds(showTime);
            ClearThoughtText();
        }
    }
    public void ClearThoughtText() {
        // Debug.Log($"ClearThoughtText");
        
        Manager_UI.Instance.ClearShowText();
        // StopAllCoroutines();
        // StartCoroutine(Manager_UI.Instance.ClearText(
        //     hideTypingSpeed
        // ));
    }    
}