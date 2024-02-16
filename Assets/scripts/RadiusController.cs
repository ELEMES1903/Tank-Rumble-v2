using UnityEngine;

public class RadiusController : MonoBehaviour
{
    // Variables for radius and adjustable rate
    public float radius = 5f;

    // Variables to track progress for Tank1 and Tank2
    public int t1Progress = 0;
    public int t2Progress = 0;

    // Flags to track presence of Tank1 and Tank2
    private bool tank1Detected = false;
    private bool tank2Detected = false;

     // Timer variables
    private float timer = 0f;
    public float interval = 1f; // Interval in seconds

    // Update is called once per frame
    void Update()
    {

        // Reset detection flags
        tank1Detected = false;
        tank2Detected = false;


        
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
        }
        else if (tank1Detected)
        {
            UpdateProgress(ref t1Progress, ref t2Progress);
        }
        else if (tank2Detected)
        {
            UpdateProgress(ref t2Progress, ref t1Progress);
        }
        else
        {
            // No tanks detected: decrease progress for both tanks

            DecreaseProgress(ref t1Progress);
            DecreaseProgress(ref t2Progress);
        }      
    }

    void UpdateProgress(ref int progressToUpdate, ref int otherProgress)
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            if (otherProgress > 0)
            {
                otherProgress -= 1;
            }
            else
            {
                if(progressToUpdate != 10){
                    progressToUpdate += 1;
                }
            }
            timer -= interval;
        }
    }

    void DecreaseProgress(ref int progress)
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            if (progress > 0 && progress != 10)
            {
                progress -= 1;
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
