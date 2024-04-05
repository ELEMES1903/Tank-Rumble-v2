using System.Collections;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    // Reference to the projectile prefab
    public GameObject projectilePrefab;

    // Speed of the projectile
    public float projectileSpeed = 10f;

    // Reload time between shots
    public float reloadTime = 1f;

    // Boolean flag to track whether the shooter is currently reloading
    private bool isReloading = false;

    // Method to shoot a projectile
    public void ShootProjectile()
    {
        // Check if the shooter is currently reloading
        if (!isReloading)
        {
            if(projectilePrefab != null){

                // Instantiate a new projectile at the current position and rotation of the shooter
                GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
                FindObjectOfType<AudioManager>().Play("Gun");

                // Get the Rigidbody2D component of the projectile
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

                // Check if the Rigidbody2D component exists
                if (rb != null)
                {
                    // Set the velocity of the projectile
                    rb.velocity = transform.up * projectileSpeed;
                }
                else
                {
                    Debug.LogWarning("Rigidbody2D component not found on the projectile prefab.");
                }
            }
            

            // Start the reload coroutine
            StartCoroutine(Reload());
        }
    }

    // Coroutine to reload the shooter after a specified duration
    private IEnumerator Reload()
    {
        // Set the flag to indicate that the shooter is currently reloading
        isReloading = true;

        // Wait for the reload time duration
        yield return new WaitForSeconds(reloadTime);

        // Reset the flag to indicate that the shooter has finished reloading
        isReloading = false;
    }

    
}
