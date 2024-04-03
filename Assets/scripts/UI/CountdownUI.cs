using UnityEngine;
using TMPro;

public class CountdownUI : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // Reference to the TextMeshProUGUI component
    public float countdownDuration = 3f; // Duration of the countdown
    public string goText = "Go!"; // Text to display after the countdown

    private float countdownTimer; // Timer for the countdown

    public bool startCountdown;

    void Start()
    {
        // Initialize the countdown timer
        countdownTimer = countdownDuration;
        FindObjectOfType<AudioManager>().Play("Game Start");
        startCountdown = true;
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
                startCountdown = false;
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
}
