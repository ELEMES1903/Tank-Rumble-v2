using UnityEngine;
using TMPro;

public class ColorSwitcher : MonoBehaviour
{
    public TextMeshProUGUI text; // Reference to the TextMeshProUGUI component
    public Color color1; // First color
    public Color color2; // Second color
    public float color1Duration = 1.0f; // Duration for color 1
    public float color2Duration = 1.0f; // Duration for color 2

    private float timer; // Timer to track color switching
    private bool isColor1; // Flag to track current color state

    void Start()
    {
        timer = 0f;
        isColor1 = true;
    }

    void Update()
    {
        // Increment timer
        timer += Time.deltaTime;

        // Check which color duration is active and switch colors accordingly
        if (isColor1 && timer >= color1Duration)
        {
            text.color = color2;
            isColor1 = false;
            timer = 0f;
        }
        else if (!isColor1 && timer >= color2Duration)
        {
            text.color = color1;
            isColor1 = true;
            timer = 0f;
        }
    }
}
