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

    private float currentSpeed;
    private Vector3 direction;
    private Rigidbody rb;
    private Transform player;
    private Vector3 targetVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("ChangeDirection", Random.Range(minChangeDirectionTime, maxChangeDirectionTime));
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Move the object smoothly
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime * movementSmoothness);

        // Check if player is within sightline
        if (CanSeePlayer())
        {
            // Rotate towards the player
            RotateTowardsPlayer();
        }
    }

    private void ChangeDirection()
    {
        // Generate a new random direction
        direction = Random.insideUnitSphere;

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

    private bool CanSeePlayer()
    {
        // Calculate direction to the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Calculate angle between AI's forward direction and direction to player
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        // Check if player is within field of view angle and within view distance
        if (angle < fieldOfViewAngle / 2f)
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
    }

    private void RotateTowardsPlayer()
    {
        // Calculate direction to the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Ignore y-axis component to only rotate in horizontal plane
        directionToPlayer.y = 0f;

        // Calculate rotation towards player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate towards player
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}