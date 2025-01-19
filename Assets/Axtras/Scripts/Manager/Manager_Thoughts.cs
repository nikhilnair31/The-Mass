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
    private Coroutine currentShowTextCoroutine;
    private bool isShowingCollisionText = false;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowText(string text, float showTime = 3f, bool isCollision = false) {
        if (currentShowTextCoroutine != null) {
            StopCoroutine(currentShowTextCoroutine);
        }

        isShowingCollisionText = isCollision;
        currentShowTextCoroutine = StartCoroutine(
            ShowTextSequence(text, showTime)
        );

    }
    private IEnumerator ShowTextSequence(string text, float showTime) {
        Manager_UI.Instance.SetShowText(text);

        if (showTime >= 0) {
            yield return new WaitForSeconds(showTime);
            ClearThoughtText();
        }
    }
    public void ClearThoughtText(bool isCollision = false) {
        if (isShowingCollisionText == isCollision) {
            currentShowTextCoroutine = null;
            Manager_UI.Instance.ClearShowText();
            isShowingCollisionText = false;
        }
    }    
}