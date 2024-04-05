using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    
    public CountdownUI countdownUI;
    
    // Health points
    public int currentHealth = 0;
    public int maxHealth = 100;
    bool playSound;
    public HealthBar healthBar;


    void Start(){

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        playSound = true;
    }

    void Update()
    {
        
        if(currentHealth <= 25 && playSound == true)
        {
            FindObjectOfType<AudioManager>().Play("LowHealth");
            playSound = false;
        }
        else if (currentHealth >= 25)
        {
            FindObjectOfType<AudioManager>().Stop("LowHealth");
            playSound = true;
        }
    }

    // Method to take damage
    public void TakeDamage(int damageAmount)
    {
        // Reduce health points
        currentHealth -= damageAmount;
        healthBar.SetHealth(currentHealth);
        FindObjectOfType<AudioManager>().Play("TakeDamage");

        // Check if health points are less than or equal to 0
        if (currentHealth <= 0)
        {
            // Destroy the GameObject
            Destroy(gameObject);
            
            FindObjectOfType<AudioManager>().Stop("LowHealth");
            FindObjectOfType<AudioManager>().Stop("Game Music");
            FindObjectOfType<AudioManager>().Play("Destroyed");
            FindObjectOfType<AudioManager>().Play("Game Over");

            countdownUI.GameInProgress = false;
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
