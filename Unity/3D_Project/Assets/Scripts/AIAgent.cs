using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class AIAgent : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float visionRange = 10f; // Distance within which the agent can see the player
    public float fieldOfViewAngle = 110f; // Angle of vision
    public float attackRange = 2f; // Distance within which the agent can attack the player
    public float pathResetDelay = 2f; // Time to wait before resetting path
    public TextMeshProUGUI giftText; // Reference to the TextMeshProUGUI element for gifts
    private NavMeshAgent agent;
    private Animator anim; // Reference to the Animator component
    private float pathResetTimer = 0f; // Timer for path reset delay
    private bool isPlayerInVision = false; // Track if the player is currently in vision
    private bool isReturningToSpawn = false; // Track if the agent is returning to spawn
    private Vector3 spawnPosition; // Store the spawn position

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>(); // Get the Animator component
        spawnPosition = transform.position; // Store the initial position as the spawn position
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (isReturningToSpawn)
        {
            // Check if the agent has reached the spawn position
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                isReturningToSpawn = false;
                // Set idle animation
                if (anim != null)
                {
                    anim.SetBool("Running", false);
                }
            }
            return;
        }

        if (IsPlayerInVision())
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
            else
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
        }
        else if (isPlayerInVision)
        {
            // Increment the timer if the player was previously in vision
            pathResetTimer += Time.deltaTime;
            if (pathResetTimer >= pathResetDelay)
            {
                agent.SetDestination(spawnPosition); // Return to spawn position
                isPlayerInVision = false; // Update vision status
                isReturningToSpawn = true; // Set returning to spawn flag

                // Set running animation
                if (anim != null)
                {
                    anim.SetBool("Running", true);
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

    private void AttackPlayer()
    {
        // Implement attack logic here
        Debug.Log("Attacking the player!");

    
        if(player.gameObject.GetComponent<GiftPickup>().HasGifts())
        {
            player.gameObject.GetComponent<GiftPickup>().UseGift();
        }
        else
        {
            player.gameObject.GetComponent<PlayerHealth>().TakeDamage(10f);
        }
            
            
       

        // After attacking, reset path to return to spawn position
        agent.SetDestination(spawnPosition);
        isPlayerInVision = false;
        isReturningToSpawn = true;

        // Set running animation
        if (anim != null)
        {
            anim.SetBool("Running", true);
        }
    }
}