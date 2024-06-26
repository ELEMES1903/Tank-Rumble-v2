using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class TankAI : MonoBehaviour
{
    // Serialized field to display the current action type in the Inspector
    [SerializeField]
    private string currentActionType;
    
    // Dictionary to store priorities for each action
    Dictionary<ActionType, float> actionPriorities = new Dictionary<ActionType, float>();

    // References
    public HealthSystem healthSystem;
    public  ProjectileShooter projectileShooter;
    public SetDestination setDestination;
    private CalculatePriority calculatePriority;
    public CountdownUI countdownUI;
    public AIPath aiPath;
    private Seeker seeker;
    private Rigidbody2D rb;
    public Animator animator;

    // Threshold distance to consider the tank has reached its destination
    public float destinationReachedThreshold = 0.1f;
    
    // Floats
    public float moveSpeed = 5f;
    public float detectionRadius = 5f;
    string selfName;
    public float enemyHealth;
    public bool canRoam;

    private bool canPlayRedEngine;
    private bool canPlayBlueEngine;



    // Enemy Objects
    string enemyName;
    float distanceToEnemy;
    public Transform enemy;

    // Healthpack Objects
    public Transform closestHP;
    float distanceToClosestHP;
    float closestDistance = 100f;

    // Capturepoint Objects
    public Transform capturepoint;
    float distanceToCapturepoint;
    
    public float checkTeam2Progress;
    public float checkTeam1Progress;

    void Start()
    {
        // If all tanks are tagged properly, set the enemy as the gameobject with the other tag
        if(gameObject.CompareTag("Tank1")||gameObject.CompareTag("Tank2")){

            if(gameObject.CompareTag("Tank1")){ enemyName = "Tank2"; selfName = "Tank1"; }
            if(gameObject.CompareTag("Tank2")){ enemyName = "Tank1"; selfName = "Tank2"; }
        } else{
            Debug.LogWarning("Tags on tanks not set properly");
        }

        // Setting references
        rb = gameObject.GetComponent<Rigidbody2D>();
        calculatePriority = gameObject.GetComponent<CalculatePriority>();
        animator = gameObject.GetComponent<Animator>();
        aiPath = gameObject.GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        
        canRoam = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(countdownUI.GameInProgress == true)
        {
            FindTargetInRadius();
            UpdatePriorities();
            ExecuteAction(); 
            RotateTowardsNextWaypoint();
        }

        if(selfName == "Tank1")
        {
            if(aiPath.hasPath && aiPath.remainingDistance > 1)
            {
                animator.SetBool("RedIsMoving", true);

                if(canPlayRedEngine)
                {
                    FindObjectOfType<AudioManager>().Play("RedEngine");
                    canPlayRedEngine = false;
                }
            }
            else if (!aiPath.hasPath)
            {
                animator.SetBool("RedIsMoving", false);
                FindObjectOfType<AudioManager>().Stop("RedEngine");
                canPlayRedEngine = true;
            }
        }

        if(selfName == "Tank2")
        {
            if(aiPath.hasPath && aiPath.remainingDistance > 1)
            {
                animator.SetBool("BlueIsMoving", true);
                if(canPlayBlueEngine)
                {
                    //FindObjectOfType<AudioManager>().Play("BlueEngine");
                    canPlayRedEngine = false;
                }
                
            }
            else if (!aiPath.hasPath) 
            {
                animator.SetBool("BlueIsMoving", false);
                //FindObjectOfType<AudioManager>().Stop("BlueEngine");
                canPlayRedEngine = true;
            }
        }
    }

        // Define actions
    enum ActionType { MoveToTarget, FindHealth, Roam, Capture}

    // Update priorities based on game state
    void UpdatePriorities()
    {
        // Example: Update priorities based on distance to target, health status, etc.
        actionPriorities[ActionType.MoveToTarget] = calculatePriority.CalculateMovePriority();
        actionPriorities[ActionType.FindHealth] = calculatePriority.CalculateFindHealthPriority();
        actionPriorities[ActionType.Roam] = calculatePriority.CalculateRoamPriority();
        actionPriorities[ActionType.Capture] = calculatePriority.CalculateCapturePriority();
    }

    // Select action with highest priority and execute it
    void ExecuteAction()
    {
        

            ActionType highestPriorityAction = GetHighestPriorityAction();
            switch (highestPriorityAction)
            {
                case ActionType.MoveToTarget:
                    MoveToTarget();
                    break;
                case ActionType.FindHealth:
                    FindHealth();
                    break;
                case ActionType.Roam:
                    Roam();
                    break;
                case ActionType.Capture:
                    Capture();
                    break;
            }
    }

    ActionType GetHighestPriorityAction()
    {
        // Find the action with the highest priority
        ActionType highestPriorityAction = ActionType.MoveToTarget;
        float highestPriority = float.MinValue;

        foreach (var kvp in actionPriorities)
        {
            if (kvp.Value > highestPriority)
            {
                highestPriority = kvp.Value;
                highestPriorityAction = kvp.Key;
            }
        }
        return highestPriorityAction;
    }

    // Method to find and set the target tank within the detection radius
    void FindTargetInRadius()
{
    // Reset variables for health packs and capture points
    closestDistance = Mathf.Infinity;
    enemy = null;
    closestHP = null;
    capturepoint = null;
    //checkTeam1Progress = 0f;
    //checkTeam2Progress = 0f;

    // Find all colliders within the detection radius
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
    
    // Loop through all colliders
    foreach (Collider2D collider in colliders)
    {
        // Check for the enemy
        if (collider.CompareTag(enemyName))
        {      
            enemy = collider.transform;
            HealthSystem enemyHealthSystem = collider.GetComponent<HealthSystem>();
            enemyHealth = enemyHealthSystem.currentHealth;

            // Calculate the distance between self and the target
            distanceToEnemy = Vector3.Distance(transform.position, enemy.position);
        }
        ///////////////////////////////////////////////////////////////////////////////////
        // Check for healthpacks
        else if (collider.CompareTag("HealthPack"))
        {
            // Calculate the distance between the tank and the health pack
            float distanceToHP = Vector3.Distance(transform.position, collider.transform.position);

            // If the current health pack is closer than the previous closest one, update the closest health pack
            if (distanceToHP < closestDistance)
            {
                closestDistance = distanceToHP;
                closestHP = collider.transform;
            }
            // Calculate the distance between self and the target
            distanceToClosestHP = Vector3.Distance(transform.position, closestHP.position);
        }
        ///////////////////////////////////////////////////////////////////////////////////
        // Check for the capture point
        else if (collider.CompareTag("Capturepoint"))
        {      
            capturepoint = collider.transform;
            RadiusController radiusController = collider.GetComponent<RadiusController>();

            // Check the points for
            checkTeam1Progress = radiusController.redProgress;
            checkTeam2Progress = radiusController.blueProgress;

            // Calculate the distance between self and the target
            distanceToCapturepoint = Vector3.Distance(transform.position, capturepoint.position);
        }
    }   
}
    

    void MoveToTarget()
    {
        // Set the target position for the AIPath component
        if(enemy != null)
        {
            aiPath.destination = enemy.position;
            aiPath.endReachedDistance = 5f;
            FireAtEnemy();        
        }
        currentActionType = "Move";
    }

    void FireAtEnemy()
    {
        
        if(enemy != null){

            // Calculate the direction to the target
            Vector3 direction = enemy.position - transform.position;

            // Ensure the direction is not zero (to avoid division by zero)
            if (direction != Vector3.zero)
            {
                // Calculate the rotation angle towards the target
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg  -   90f;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Smoothly rotate the tank towards the target
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 300f * Time.deltaTime);
            }

            projectileShooter.ShootProjectile();    

        }
    }

    void FindHealth()
    {
        if(closestHP != null)
        {
            aiPath.destination = closestHP.position;
            aiPath.endReachedDistance = 0f;
            
        }
        currentActionType = "FindHealth";
    }


    void Capture()
    {
        if(capturepoint != null){

            aiPath.destination = capturepoint.position;   
        }

        currentActionType = "Capture";
    }

    void Roam()
    {
        aiPath.endReachedDistance = 0f;
        if(canRoam == true)
        {
            SetRandomDestination();
            canRoam = false;
        }
        else
        {
            // Check if the tank has reached its destination
            if (IsDestinationReached())
            {
                canRoam = true;
            }
        }
        currentActionType = "Roam";
    }
    
    // Method to set a random destination for the tank
    void SetRandomDestination()
    {
        // Generate a random position using the SetDestination script
        Vector2 randomPosition = setDestination.GenerateRandomPosition();

        // Set the destination of the AIPath component to the random position
        aiPath.destination = randomPosition;
    }

    // Method to check if the tank has reached its destination
    bool IsDestinationReached()
    {
        // Calculate the distance between the tank and its destination
        float distanceToDestination = Vector2.Distance(transform.position, aiPath.destination);

        // Check if the distance is smaller than the threshold
        return distanceToDestination < destinationReachedThreshold;
    }

    // Visualize the detection radius in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    
    void RotateTowardsNextWaypoint()
    {
        // Check if AIPath has a valid path
        if (aiPath.hasPath)
        {
            

            // Get the direction to the next waypoint along the AIPath
            Vector2 direction = (Vector2)aiPath.steeringTarget - (Vector2)transform.position;

            // Ensure the direction is not zero (to avoid division by zero)
            if (direction != Vector2.zero)
            {
                // Calculate the rotation angle towards the next waypoint
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                
                // Create a Quaternion from the angle
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Smoothly rotate the tank towards the next waypoint
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 300f * Time.deltaTime);
            }
        }
    }
}
