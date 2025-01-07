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
        lookedAtText.text = text;

        for (int lettersDisplayed = 0; lettersDisplayed <= text.Length; lettersDisplayed++) {
            lookedAtText.maxVisibleCharacters = lettersDisplayed;

            AudioClip clip = typingClips[Random.Range(0, typingClips.Length)];
            uiSource.PlayOneShot(clip);

            yield return new WaitForSeconds(showTypingSpeed);
        }
    }
    public IEnumerator ClearText() {
        int textLen = lookedAtText.text.Length;
        for (int lettersDisplayed = textLen; lettersDisplayed >= 0; lettersDisplayed--) {
            lookedAtText.maxVisibleCharacters = lettersDisplayed;

            yield return new WaitForSeconds(hideTypingSpeed);
        }

        lookedAtText.text = "";
    }
    
}