using UnityEngine;
using System.Collections;
using TMPro;

public class Manager_UI : MonoBehaviour 
{
    #region Vars
    public static Manager_UI Instance { get; private set; }

    [Header("Look At UI")]
    [SerializeField] private AudioSource uiSource;
    [SerializeField] private AudioClip[] typingClips;
    [SerializeField] private TMP_Text lookedAtText;
    [SerializeField] private float showTypingSpeed = 0.05f;
    [SerializeField] private float hideTypingSpeed = 0.02f;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public IEnumerator ShowTextWithSound(string text) {
        lookedAtText.text = "";

        foreach (char c in text) {
            lookedAtText.text += c;

            if (typingClips.Length > 0) {
                AudioClip clip = typingClips[Random.Range(0, typingClips.Length)];
                uiSource.PlayOneShot(clip);
            }

            yield return new WaitForSeconds(showTypingSpeed);
        }
    }
    public IEnumerator ClearText() {
        while (!string.IsNullOrEmpty(lookedAtText.text)) {
            lookedAtText.text = lookedAtText.text[..^1];

            if (typingClips.Length > 0) {
                AudioClip clip = typingClips[Random.Range(0, typingClips.Length)];
                uiSource.PlayOneShot(clip);
            }

            yield return new WaitForSeconds(hideTypingSpeed);
        }

        lookedAtText.text = "";
    }
    
}