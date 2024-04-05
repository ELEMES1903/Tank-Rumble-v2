using UnityEngine;

public class RadiusController : MonoBehaviour
{
    // Variables for radius and adjustable rate
    public float radius = 5f;

    // Variables to track progress for Tank1 and Tank2
    public int redProgress = 0;
    public int blueProgress = 0;
    public bool redPoint;
    public bool bluePoint;

    // Flags to track presence of Tank1 and Tank2
    private bool tank1Detected = false;
    private bool tank2Detected = false;

     // Timer variables
    private float timer = 0f;
    public float interval = 1f; // Interval in seconds


    // Sprites
    public Sprite redSprite;
    public Sprite blueSprite;

    // Reference to the sprite renderer
    private SpriteRenderer spriteRenderer;

    public bool captured; 
    private bool canPlaySound;

    public bool contested = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        canPlaySound = true;
    }

    // Update is called once per frame
    void Update()
    {

        // Reset detection flags
        tank1Detected = false;
        tank2Detected = false;

        captured = false;
        
        // Check for tanks within the radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {

            if (collider.CompareTag("Tank1"))
            {
                tank1Detected = true;
            }
            else if (collider.CompareTag("Tank2"))
            {
                tank2Detected = true;
            }
        }    
            
        // Update progress based on tank detections
        if (tank1Detected && tank2Detected)
        {
            // Both tanks detected: no progress change
            FindObjectOfType<AudioManager>().Play("InterruptCapture");
            contested = true;
        }
        else if (tank1Detected)
        {
            UpdateCaptureStatus(ref redProgress, ref blueProgress, ref redPoint, ref bluePoint);
            ManualUpdateProgress(ref redProgress, ref blueProgress, ref redPoint, ref bluePoint);
            contested = false;
        }
        else if (tank2Detected)
        {
            UpdateCaptureStatus(ref blueProgress, ref redProgress, ref bluePoint, ref redPoint);
            ManualUpdateProgress(ref blueProgress, ref redProgress, ref bluePoint, ref redPoint);
            contested = false;
        }
        else
        {
            AutoUpdateProgress(ref redProgress, ref redPoint);
            AutoUpdateProgress(ref blueProgress, ref bluePoint);
            contested = false;
        }      

        if(tank1Detected||tank2Detected)
        {
            if(canPlaySound)
            {
                FindObjectOfType<AudioManager>().Play("Capturing");
                canPlaySound = false;
            }
        }
        else if (!tank1Detected && !tank2Detected)
        {
           canPlaySound = true;
           FindObjectOfType<AudioManager>().Stop("Capturing");
        }

    }


    void UpdateCaptureStatus(ref int allyProgress, ref int enemyProgress, ref bool allyPoint, ref bool enemyPoint)
    {
        if (allyPoint == false && enemyPoint == false && allyProgress == 10)
        {
            allyPoint = true;
            allyProgress = 10;

            captured = true;
            enemyPoint = false;
        }

        if (allyPoint == false && enemyPoint == true && enemyProgress == 0)
        {
            allyPoint = true;
            allyProgress = 10;

            captured = true;
            enemyPoint = false;
        }

        //update capturepoint sprite and activate sfx
        if(redPoint == true && captured == true)
        {
            spriteRenderer.sprite = redSprite;
            FindObjectOfType<AudioManager>().Play("Capture");

        }
        else if(bluePoint == true && captured == true)
        {
            spriteRenderer.sprite = blueSprite;
            FindObjectOfType<AudioManager>().Play("Capture");

        }
    }

    void ManualUpdateProgress(ref int allyProgress, ref int enemyProgress, ref bool allyPoint, ref bool enemyPoint)
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            // if condition that only works on neutral points, points that has never been captured
            if (allyPoint == false && enemyPoint == false && enemyProgress > 0)
            {
                enemyProgress -= 1;
            }
            else if (allyPoint == false && enemyPoint == false && allyProgress < 10)
            {
                allyProgress += 1;
            }
            

            if (allyPoint == false && enemyPoint == true && enemyProgress > 0)
            {
                enemyProgress -= 1;
            }
            

            timer -= interval;
        }
    }

    void AutoUpdateProgress(ref int allyProgress, ref bool allyPoint)
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            //if the ALLY controls the point and the ENEMY partially progressed in capturing the point and the ENEMY is no longer capturing, then slowly revert the progress
            if (allyProgress < 10 && allyPoint == true)
            {
                allyProgress += 1;
            }
            
            //if the ENEMY controls the point and the ALLY partially progressed in capturing the point and the ALLY is no longer capturing, then slowly revert the progress
            if (allyProgress > 0 && allyPoint == false)
            {
                allyProgress -= 1;
            }

            timer -= interval;
        }
    }


    // Draw the collider in Scene view when selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
