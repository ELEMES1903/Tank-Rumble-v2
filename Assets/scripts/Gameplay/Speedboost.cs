using UnityEngine;
using Pathfinding;

public class SpeedBoost : MonoBehaviour
{
    // Reference to the AIPath component
    private AIPath aiPath;

    // Speed boost variables
    public float speedBoostDuration = 2f; // Duration of the speed boost
    public float speedBoostCooldown = 10f; // Cooldown period for the speed boost
    public float speedBoostMultiplier = 1.5f; // Multiplier for speed boost

    // Cooldown timer variables
    private float currentCooldownTimer = 0f;
    private bool isSpeedBoostActive = false;

    void Start()
    {
        // Get the AIPath component attached to the tank
        aiPath = GetComponent<AIPath>();

        // Ensure the AIPath component is not null
        if (aiPath == null)
        {
            Debug.LogError("AIPath component not found on the tank GameObject.");
            return;
        }
    }

    void Update()
    {
        // Update cooldown timer
        if (!isSpeedBoostActive)
        {
            currentCooldownTimer -= Time.deltaTime;
        }

        // Check if the speed boost is available and not active
        if (currentCooldownTimer <= 0 && !isSpeedBoostActive)
        {
            // Activate speed boost
            ActivateSpeedBoost();
        }

        // Check if the speed boost is active
        if (isSpeedBoostActive)
        {
            // Reduce the duration of speed boost
            speedBoostDuration -= Time.deltaTime;

            // Check if the duration has elapsed
            if (speedBoostDuration <= 0)
            {
                // Deactivate speed boost
                DeactivateSpeedBoost();
            }
        }
    }

    // Method to activate the speed boost
    void ActivateSpeedBoost()
    {
        // Apply speed boost multiplier to AIPath speed
        aiPath.maxSpeed *= speedBoostMultiplier;

        // Set speed boost active flag
        isSpeedBoostActive = true;

        // Reset cooldown timer
        currentCooldownTimer = speedBoostCooldown;

        Debug.Log("Speed boost activated!");
    }

    // Method to deactivate the speed boost
    void DeactivateSpeedBoost()
    {
        // Reset AIPath speed to default
        aiPath.maxSpeed /= speedBoostMultiplier;

        // Set speed boost inactive flag
        isSpeedBoostActive = false;

        // Reset speed boost duration
        speedBoostDuration = 0f;

        Debug.Log("Speed boost deactivated!");
    }
}
