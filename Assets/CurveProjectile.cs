using UnityEngine;

public class CurveProjectile : MonoBehaviour
{
    public float gravity = 9.8f; // Gravity value
    public Transform target; // Target position
    public Vector3 initialPosition; // Initial position
    public float time = 1.0f; // Time it takes to reach the target
    public float height = 2.0f; // Height of the curve

    private Vector3 velocity; // Initial velocity of the object
    private float startTime; // Start time of the curve

    private void Start()
    {
        initialPosition = transform.position;
        // Calculate the initial velocity
        Vector3 displacement = target.position - initialPosition;
        velocity = displacement / time;

        // Calculate the initial y velocity to achieve the desired height
        velocity.y = (height - initialPosition.y + target.position.y - 0.5f * gravity * time * time) / time;

        // Record the start time
        startTime = Time.time;
    }

    private void Update()
    {
        // Calculate the current time
        float currentTime = Time.time - startTime;

        // Calculate the current position using the equation of motion for a projectile
        Vector3 currentPosition = initialPosition + velocity * currentTime;
        currentPosition.y = initialPosition.y + velocity.y * currentTime - 0.5f * gravity * currentTime * currentTime;

        // Move the object to the current position
        transform.position = currentPosition;
    }
}
