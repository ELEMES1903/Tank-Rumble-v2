using Pathfinding;
using UnityEngine;

public class HealthBooster : MonoBehaviour
{

    // Method to handle boosting health when a tank touches the GameObject
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding GameObject is a tank
        if (other.CompareTag("Tank1")||other.CompareTag("Tank2"))
        {
            // Get the HealthSystem component of the tank
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            
            // Check if the HealthSystem component exists
            if (healthSystem != null)
            {
                // Add 10 to the tank's health
                healthSystem.AddHealth(10);

                // Get the AIPath component of the tank
                AIPath aiPath = other.GetComponent<AIPath>();
                // Set the tanks endReachedDistance back to normal
                aiPath.endReachedDistance = 10f;

                // Destroy the health booster after use
                Destroy(gameObject);
            }
        }
    }
}
