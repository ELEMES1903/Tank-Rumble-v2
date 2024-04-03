using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // Health points
    public int currentHealth = 0;
    public int maxHealth = 100;

    public HealthBar healthBar;


    void Start(){

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }


    // Method to take damage
    public void TakeDamage(int damageAmount)
    {
        // Reduce health points
        currentHealth -= damageAmount;
        healthBar.SetHealth(currentHealth);

        // Check if health points are less than or equal to 0
        if (currentHealth <= 0)
        {
            // Destroy the GameObject
            Destroy(gameObject);
        }
    }

    // Method to add health points
    public void AddHealth(int amount)
    {
        // Add the specified amount to the current health
        currentHealth += amount;

        if(currentHealth > maxHealth){
            currentHealth = maxHealth;
        }

        healthBar.SetHealth(currentHealth);

    }











}
