using UnityEngine;
using TMPro;

public class Controller_Clock : MonoBehaviour
{
    #region Vars
    public static Controller_Clock Instance { get; private set; }

    [Header("Clock Settings")]
    [SerializeField] private TMP_Text clockTimeText;
    [SerializeField] private bool isClockRunning = true;
    [SerializeField] private float timeMultiplier = 6f;  // 1 real second = 6 in-game seconds
    private int hour = 22;  // 10 PM in 24-hour format
    private int minute = 52;
    private float timer;
    #endregion

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update() {
        if (isClockRunning) {
            ClockUpdate();
        }
    }
    private void ClockUpdate() {
        // Increment the timer based on Time.deltaTime
        timer += Time.deltaTime * timeMultiplier;

        // When 10 seconds pass in real time, add 1 minute
        if (timer >= 60f) {
            timer = 0f;
            
            minute++;

            // If minutes exceed 59, reset to 0 and add 1 to the hour
            if (minute >= 60) {
                minute = 0;
                hour++;

                // If hours exceed 23, reset to 0 (next day)
                if (hour >= 24) {
                    hour = 0;
                }
            }
        }

        // Update the clock display
        string period = hour >= 12 ? "PM" : "AM";
        int displayHour = hour % 12;
        displayHour = displayHour == 0 ? 12 : displayHour;  // Handle 12-hour format

        clockTimeText.text = $"{displayHour:D2}:{minute:D2} {period}";
    }

    public void StopClock() {
        isClockRunning = false;
    }
}
