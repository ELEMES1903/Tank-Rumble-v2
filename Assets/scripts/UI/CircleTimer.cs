using UnityEngine;
using UnityEngine.UI;

public class RadialTimer : MonoBehaviour
{
    public float totalTime = 60f; // Total time for the timer
    public Image timerImage; // Reference to the UI Image object
    private float currentTime = 0f; // Current time of the timer

    void Update()
    {
        // Update the timer
        if (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            float fillAmount = currentTime / totalTime;
            timerImage.fillAmount = fillAmount;
        }
        else
        {
            // Timer has finished
            // You can add code here to handle timer completion
        }
    }
}
