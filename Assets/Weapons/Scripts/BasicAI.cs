using UnityEngine;

public class BasicAI : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 5f;
    public float minChangeDirectionTime = 1f;
    public float maxChangeDirectionTime = 3f;
    public float rotationSpeed = 3f;
    public float fieldOfViewAngle = 45f;
    public float viewDistance = 10f;
    public float movementSmoothness = 5f;
    public float followDelay = 2f; // Delay before the enemy stops following the player
    private float followTimer = 0f; // Timer to track follow delay

    private float currentSpeed;
    private Rigidbody rb;
    private Transform player;
    private Vector3 targetVelocity;
    private bool isFollowingPlayer = false; // Flag to track if AI is following the player
    private bool isPlayerVisible;
    public bool followingPlayer;
    public float followTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("ChangeDirection", Random.Range(minChangeDirectionTime, maxChangeDirectionTime));
        player = GameObject.FindGameObjectWithTag("Player").transform;
        followingPlayer = isFollowingPlayer;
        followTime = followTimer;
        CollisionHandler.OnCollisionOccured += CanSeePlayer;
    }

    private void CanSeePlayer(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerVisible = true;
            Debug.LogWarning("Player Seen!");
        } else { isPlayerVisible = false; }
    }

    private void Update()
    {
        // Move the object smoothly
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime * movementSmoothness);

        // Check if player is within sightline
        if (isPlayerVisible)
        {
            // Track the player's position
            TrackPlayer();
            isFollowingPlayer = true;
            followTimer = followDelay; // Reset follow delay timer
        }
        else if (isFollowingPlayer)
        {
            // If not visible but still following, decrement follow delay timer
            followTimer -= Time.deltaTime;
            if (followTimer <= 0f)
            {
                // Stop following after the delay
                isFollowingPlayer = false;
            }
        }
        else
        {
            // Rotate naturally if not following player
            //RotateNaturally();
        }

        if (isFollowingPlayer)
        {
            // Move towards the player
            MoveTowardsPlayer();
        }

        Debug.Log("Is Player Visible: " + isPlayerVisible);
    }

    private void ChangeDirection()
    {
        // Generate a new random direction
        Vector3 direction = Random.insideUnitSphere;

        // Set the y-component to 0 to restrict movement on the y-axis
        direction.y = 0f;

        // Normalize the direction to ensure consistent speed
        direction.Normalize();

        // Generate a new random speed
        currentSpeed = Random.Range(minSpeed, maxSpeed);

        // Set the target velocity based on the new direction and speed
        targetVelocity = direction * currentSpeed;

        // Schedule the next direction change
        Invoke("ChangeDirection", Random.Range(minChangeDirectionTime, maxChangeDirectionTime));
    }

    /*private bool CanSeePlayer()
    {
        // Calculate direction to the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Calculate angle between AI's forward direction and direction to player
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        // Check if player is within field of view angle and within view distance

        coneMeshCollider.

        if (angle < fieldOfViewAngle / 1f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, viewDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }*/

    private void TrackPlayer()
    {
        // Calculate direction to the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Ignore y-axis component to only rotate in horizontal plane
        directionToPlayer.y = 0f;

        // Calculate rotation towards player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Set the rotation to directly face the player
        //transform.rotation = targetRotation;
        RotateNaturally(directionToPlayer);
    }
    private void RotateNaturally()
    {
        // Rotate naturally (e.g., random direction)
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    private void RotateNaturally(Vector3 direction)
    {
        // Rotate naturally (e.g., random direction)
        transform.Rotate(direction, rotationSpeed * Time.deltaTime);
    }

    private void MoveTowardsPlayer()
    {
        // Calculate direction to the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Set the target velocity to move towards the player
        targetVelocity = directionToPlayer.normalized * currentSpeed;
    }
}
