using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
public class CapturePointManager : MonoBehaviour
{
    // References to the Capture Points
    public RadiusController pointA;
    public RadiusController pointB;
    public RadiusController pointC;

    // Text and Image components for each point
    public TextMeshProUGUI textA;
    public TextMeshProUGUI textB;
    public TextMeshProUGUI textC;

    public GameObject imageA;
    public GameObject imageB;
    public GameObject imageC;
    public float Duration = 1.0f; // Duration for color 1
    private float timer; // Timer to track color switching
    private bool isColor1; // Flag to track current color state

    void Update()
    {
        UpdatePointStatus(pointA, textA, imageA);
        UpdatePointStatus(pointB, textB, imageB);
        UpdatePointStatus(pointC, textC, imageC);
    }

    void UpdatePointStatus(RadiusController point, TextMeshProUGUI text, GameObject image)
    {
        // Check if the point is captured by red team
        if (point.redPoint)
        {
            // Set text color and value
            text.color = Color.red;
            text.text = point.redProgress.ToString() + "0%";

            StartFlashImage(image, Color.red, point.redProgress, point.blueProgress, point);
        }
        // Check if the point is captured by blue team
        else if (point.bluePoint)
        {
            // Set text color and value
            text.color = Color.blue;
            text.text = point.blueProgress.ToString() + "0%";

            StartFlashImage(image, Color.blue, point.blueProgress, point.redProgress, point);
        }
        else
        {
            StartFlashImage(image, Color.white, point.blueProgress, point.redProgress, point);

            if(point.redProgress > point.blueProgress)
            {
                text.text = point.redProgress.ToString() + "0%";
                text.color = Color.red;
            }
            else if (point.blueProgress > point.redProgress)
            {
                text.text = point.blueProgress.ToString() + "0%";
                text.color = Color.blue;
            }
            else
            {
                text.color = Color.white;
                text.text = "0%";    
            }     
        }
    }

    // Call this method when you want to flash the image conditionally
    void StartFlashImage(GameObject image, Color color, int allyProgress, int enemyProgress, RadiusController point)
    {
        if (color != Color.blue && color != Color.red && point.contested)
        {
            FlashImage(image, color);
            return;
        }
        
        if (allyProgress > enemyProgress && point.contested)
        {
            FlashImage(image, color);
        } 
        else
        {
            image.GetComponent<Image>().color = color;
        }
    }

    void FlashImage(GameObject image, Color color)
    {
        // Increment timer
        timer += Time.deltaTime;

        // Check which color duration is active and switch colors accordingly
        if (isColor1 && timer >= Duration)
        {
            image.GetComponent<Image>().color = Color.black;
            isColor1 = false;
            timer = 0f;
        }
        else if (!isColor1 && timer >= Duration)
        {
            image.GetComponent<Image>().color = color;
            isColor1 = true;
            timer = 0f;
        }
    }
}
