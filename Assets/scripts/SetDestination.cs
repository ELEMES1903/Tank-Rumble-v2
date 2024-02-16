using UnityEngine;
using UnityEngine.UIElements;

public class SetDestination : MonoBehaviour
{
    public Vector2 areaSize = new Vector2(10f, 10f); // Size of the area

    // Method to generate a random position within the specified area
    public Vector2 GenerateRandomPosition()
    {
        Vector2 randomPosition = Vector2.zero;
        bool positionValid = false;

        // Continue generating positions until a valid one is found
        while (!positionValid)
        {
            // Generate a random position within the area
            randomPosition.x = Random.Range(transform.position.x - areaSize.x / 2f, transform.position.x + areaSize.x / 2f);
            randomPosition.y = Random.Range(transform.position.y - areaSize.y / 2f, transform.position.y + areaSize.y / 2f);

            // Check if there are any colliders within a certain radius of the random position
            Collider2D[] colliders = Physics2D.OverlapCircleAll(randomPosition, 0.5f);
            if (colliders.Length == 0)
            {
                // No colliders found, position is valid
                positionValid = true;
            }
        }

        return randomPosition;
    }

    // Draw gizmo to visualize the spawn area in the scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
