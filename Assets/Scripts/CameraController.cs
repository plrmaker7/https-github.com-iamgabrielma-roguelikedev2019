using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The main problem we try to solve here is that the Main Camera is a game object that is already in place before the Player Entity is generated, so it cannot be assigned as the Player instance is null */
public class CameraController : MonoBehaviour
{
    public Transform target;
    private bool isPlayerFound;

    void Start()
    {
        isPlayerFound = false; // Will be false by default as the Camera is already in the Inspector before the Player Entity is instantiated.
        // TODO: Value 12 for the camera size hardcoded for the moment, calculate this dynamically in the future.
        this.GetComponent<Camera>().orthographicSize = 12;

    }

    private void Update()
    {
        if (target != null)
        {
            return; // If the Player has been found means that its Transform != null, so we can get out of the statement

        }
        else if (target == null && isPlayerFound == false)
        {
            // Set camera:
            //target = GameObject.FindWithTag("Player").transform; 
            target = Engine.__player.transform; // Find the Player
            gameObject.transform.SetParent(target); // Assign the camera game object as a child of the Player Transform
            gameObject.transform.localPosition = new Vector3(0,0,-1); // Reset its position to 0,0,-1 from the parent game object
            isPlayerFound = true; // Switch the bool so this is not triggered again
        }
    }

}
