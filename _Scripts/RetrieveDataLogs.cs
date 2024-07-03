using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveDataLogs : MonoBehaviour
{
    private Vector3 previousPosition;
    private Vector3 previousVelocity;
    public GameObject rover;
    private Vector3 currentVelocity;
    private float distanceMoved;

    void Start()
    {
        previousPosition = transform.position;
        previousVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        Vector2 displacement = currentPosition - previousPosition;
        
        // Calculate velocity as displacement per second
        currentVelocity = displacement / Time.deltaTime;

        if (Mathf.Sign(currentVelocity.x) != Mathf.Sign(previousVelocity.x) || Mathf.Sign(currentVelocity.y) != Mathf.Sign(previousVelocity.y)) {
            // Calculate the distance moved since the start
            distanceMoved = Vector3.Distance(previousPosition, currentPosition);

            // Print the distance moved to the console
            Debug.Log("Distance Moved: " + distanceMoved);

            // Reset previousPosition;
            previousPosition = currentPosition;
            
        }

        // Debugging
        //Debug.Log("Velocity: " + currentVelocity);

        // Update previousPosition to currentPosition for the next frame
        
        previousVelocity = currentVelocity;
    }
}
