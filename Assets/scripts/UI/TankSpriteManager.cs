using UnityEngine;

public class TankSpriteMana : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    public Sprite[] animationSprites; // Array of sprites for the animation
    public float minVelocityThreshold = 0.1f; // Minimum velocity threshold to play the animation

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private bool isMoving = false; // Flag to track if the tank is moving

    void Start()
    {
        // Get the Rigidbody2D component attached to the tank
        rb = GetComponent<Rigidbody2D>();

        // Ensure the SpriteRenderer component is not null
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer component not found on the tank GameObject.");
                return;
            }
        }

        // Ensure animationSprites array is not empty
        if (animationSprites.Length == 0)
        {
            Debug.LogError("AnimationSprites array is empty. Please assign sprites for the animation.");
            return;
        }
    }

    void Update()
    {
        // Check if the tank is moving based on its velocity
        if (rb.velocity.magnitude > minVelocityThreshold)
        {
            // If the tank is moving, play the animation
            if (!isMoving)
            {
                PlayAnimation();
            }
        }
        else
        {
            // If the tank is not moving, stop the animation
            if (isMoving)
            {
                StopAnimation();
            }
        }
    }

    // Method to play the animation
    void PlayAnimation()
    {
        isMoving = true;
        // Start the animation by looping through the animation sprites
        StartCoroutine(PlayAnimationCoroutine());
    }

    // Coroutine to loop through the animation sprites
    System.Collections.IEnumerator PlayAnimationCoroutine()
    {
        while (true)
        {
            foreach (Sprite sprite in animationSprites)
            {
                spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(0.1f); // Adjust delay as needed
            }
        }
    }

    // Method to stop the animation
    void StopAnimation()
    {
        isMoving = false;
        StopAllCoroutines(); // Stop any ongoing animation coroutine
    }
}
