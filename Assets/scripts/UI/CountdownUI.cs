using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownUI : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // Reference to the TextMeshProUGUI component
    public float countdownDuration = 3f; // Duration of the countdown
    public string goText = "Go!"; // Text to display after the countdown
    private float countdownTimer; // Timer for the countdown
    public bool startCountdown;
    public bool GameInProgress = false;
    public bool GameRunning;

    void Start()
    {
        // Initialize the countdown timer
        countdownTimer = countdownDuration;
        GameRunning = false;
    }

    void Update()
    {
        if(startCountdown == true)
        {
            // Update the countdown
            countdownTimer -= Time.deltaTime;

            // Check if the countdown has finished
            if (countdownTimer <= 0f)
            {
                countdownText.text = goText; // Set the text to "Go!"
                GameInProgress = true;
                
                StartCoroutine(CompleteCountdownAfterDelay(0.5f)); // Start coroutine to complete countdown after a delay
                return;
            }

            // Update the text to display the current countdown number
            int countdownNumber = Mathf.CeilToInt(countdownTimer);
            countdownText.text = countdownNumber.ToString();

        }    
        else
        {
            // If not counting down, set the text to an empty string
            countdownText.text = "";
        }
    }

    public void InitiateCountdown()
    {
        if(GameRunning == false)
        {
            startCountdown = true;
            GameRunning = true;
            FindObjectOfType<AudioManager>().Play("Game Start");
        }
        
    }

    IEnumerator CompleteCountdownAfterDelay(float delay)
    {
        float startTime = Time.realtimeSinceStartup; // Get the start time

        // Loop until the delay has passed
        while (Time.realtimeSinceStartup - startTime < delay)
        {
            yield return null; // Wait for the next frame
        }

        // Once the delay has passed, set startCountdown to false
        startCountdown = false;
        FindObjectOfType<AudioManager>().Play("Game Music");
    }

}
