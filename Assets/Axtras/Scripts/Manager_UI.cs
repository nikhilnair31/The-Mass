using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Manager_UI : MonoBehaviour 
{
    #region Vars
    public static Manager_UI Instance { get; private set; }

    [Header("Game UI")]
    [SerializeField] private GameObject gameCanvasGO;
    private bool inGame = true;

    [Header("Pause UI")]
    [SerializeField] private GameObject pauseCanvasGO;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameoverCanvasGO;
    [SerializeField] private Button restartGameButtonGO;
    [SerializeField] private Button exitGameButtonGO;
    private bool gameOver = false;

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
    
    private void Start() {
        restartGameButtonGO.onClick.AddListener(RestartGame);
        exitGameButtonGO.onClick.AddListener(ExitGame);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }
    }
    
    public void PauseGame() {
        if (inGame) {
            inGame = false;
            gameCanvasGO.SetActive(false);
            pauseCanvasGO.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else {
            inGame = true;
            gameCanvasGO.SetActive(true);
            pauseCanvasGO.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void GameOver() {
        gameOver = true;
        gameCanvasGO.SetActive(false);
        pauseCanvasGO.SetActive(false);
        gameoverCanvasGO.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void RestartGame() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  
    }
    public void ExitGame() {
        Application.Quit();
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