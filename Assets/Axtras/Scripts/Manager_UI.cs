using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

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
    [SerializeField] private TMP_Text lookedAtText;
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

        Time.timeScale = 1f;
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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  
        Time.timeScale = 1f;
    }
    public void ExitGame() {
        Application.Quit();
    }

    public IEnumerator ShowTextWithSound(AudioSource source, AudioClip[] clips, float speed, string text) {
        lookedAtText.text = text;

        for (int lettersDisplayed = 0; lettersDisplayed <= text.Length; lettersDisplayed++) {
            lookedAtText.maxVisibleCharacters = lettersDisplayed;

            AudioClip clip = clips[Random.Range(0, clips.Length)];
            source.PlayOneShot(clip);

            yield return new WaitForSeconds(speed);
        }
    }
    public IEnumerator ClearText(float speed) {
        int textLen = lookedAtText.text.Length;
        for (int lettersDisplayed = textLen; lettersDisplayed >= 0; lettersDisplayed--) {
            lookedAtText.maxVisibleCharacters = lettersDisplayed;

            yield return new WaitForSeconds(speed);
        }

        lookedAtText.text = "";
    }   
}