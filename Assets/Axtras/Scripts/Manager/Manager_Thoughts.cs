using System.Collections;
using UnityEngine;

public class Manager_Thoughts : MonoBehaviour 
{
    #region Vars
    public static Manager_Thoughts Instance { get; private set; }

    public enum TextPriority { None, Item, Collider, Player }
    
    [Header("Thoughts Settings")]
    [SerializeField] private AudioSource thoughtsAudioSource;
    [SerializeField] private AudioClip[] typingClips;
    [SerializeField] private float showTypingSpeed = 0.05f;
    [SerializeField] private float hideTypingSpeed = 0.02f;
    private TextPriority currentTextPriority = TextPriority.None;
    private Coroutine currentShowTextCoroutine;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowText(string text, float showTime, TextPriority priority) {
        if (currentShowTextCoroutine != null) {
            StopCoroutine(currentShowTextCoroutine);
        }

        currentTextPriority = priority;
        currentShowTextCoroutine = StartCoroutine(
            ShowTextSequence(text, showTime)
        );

    }
    private IEnumerator ShowTextSequence(string text, float showTime) {
        Manager_UI.Instance.SetShowText(text);

        if (showTime >= 0) {
            yield return new WaitForSeconds(showTime);
            ClearThoughtText(TextPriority.Item);
        }
    }
    public void ClearThoughtText(TextPriority priority) {
        if (currentTextPriority == priority) {
            currentShowTextCoroutine = null;
            currentTextPriority = TextPriority.None;
            Manager_UI.Instance.ClearShowText();
        }
    }    
}