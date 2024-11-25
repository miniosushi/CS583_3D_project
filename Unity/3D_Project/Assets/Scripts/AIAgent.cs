using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float visionRange = 10f; // Distance within which the agent can see the player
    public float fieldOfViewAngle = 110f; // Angle of vision
    public float pathResetDelay = 2f; // Time to wait before resetting path
    private NavMeshAgent agent;
    private Animator anim; // Reference to the Animator component
    private float pathResetTimer = 0f; // Timer for path reset delay
    private bool isPlayerInVision = false; // Track if the player is currently in vision

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if(anim != null)
        anim = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        if (IsPlayerInVision())
        {
            agent.SetDestination(player.position); // Move towards the player
            pathResetTimer = 0f; // Reset the timer if the player is in vision
            isPlayerInVision = true; // Update vision status

            // Set running animation
            if (anim != null)
            {
                anim.SetBool("Running", true);
            }
        }
        else if (isPlayerInVision)
        {
            // Increment the timer if the player was previously in vision
            pathResetTimer += Time.deltaTime;
            if (pathResetTimer >= pathResetDelay)
            {
                agent.ResetPath(); // Reset path after the delay
                isPlayerInVision = false; // Update vision status

                // Set idle animation
                if (anim != null)
                {
                    anim.SetBool("Running", false);
                }
            }
        }
        else
        {
            // Set idle animation if not chasing
            if (anim != null)
            {
                anim.SetBool("Running", false);
            }
        }
    }

    private bool IsPlayerInVision()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        // Check if the player is within the vision range and angle
        if (directionToPlayer.magnitude < visionRange && angle < fieldOfViewAngle / 2)
        {
            RaycastHit hit;
            // Check if there is a clear line of sight to the player
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, visionRange))
            {
                if (hit.transform == player)
                {
                    return true; // Player is in vision
                }
            }
        }
        return false; // Player is not in vision
    }
}