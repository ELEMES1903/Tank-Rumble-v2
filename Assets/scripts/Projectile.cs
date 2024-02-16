using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    // Method to handle damage to tanks when the projectile hits them
    private void OnTriggerEnter2D(Collider2D other)
    {       
        // Check if the projectile hit a tank (exclude self)
        if (other.CompareTag("Tank1")||other.CompareTag("Tank2"))
        {
            // Find the HealthSystem script of the tank and call TakeDamage(10) method
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(10);
            }
        }
        
        // Ignore collisions with other projectiles
        if (other.gameObject.CompareTag("Projectile")||other.gameObject.CompareTag("Capturepoint"))
        {
            Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());
            return; // Exit the method to prevent the projectile from being destroyed
        }

        // Destroy the projectile if it collides with anything other than a tank or another projectile
        Destroy(gameObject);
            
    }
}
