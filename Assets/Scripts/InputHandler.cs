﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private bool isPlayerMoving; // This avoids that there's multiple movement until the previous movement hasn't completed

    GameObject player; // Self reference, in order to use MovePlayer() easily
    private Vector3 _lastKnownPlayerPosition; // We use this to track players last position before their next move

    private void Start()
    {
        // TODO: add nullcheck. We assign player to the static __player instance, shouldn't be null but check for safety.
        player = Engine.__player; 
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.W) && isPlayerMoving == false)
        {
            isPlayerMoving = true;
            MovePlayer("up");
        }
        else if (Input.GetKeyDown(KeyCode.S) && isPlayerMoving == false)
        {
            isPlayerMoving = true;
            MovePlayer("down");
        }
        else if (Input.GetKeyDown(KeyCode.D) && isPlayerMoving == false)
        {
            isPlayerMoving = true;
            MovePlayer("right");
        }
        else if (Input.GetKeyDown(KeyCode.A) && isPlayerMoving == false)
        {
            isPlayerMoving = true;
            MovePlayer("left");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // TODO: Will implement game menu screen here in the future.
        }

    }

    public void MovePlayer(string direction)
    {
        _lastKnownPlayerPosition = player.transform.localPosition; 

        switch (direction)
        {
            case "up":
                float yPositive = player.transform.localPosition.y + 1.0f;
                player.transform.localPosition = new Vector3(player.transform.localPosition.x, yPositive, 0);
                break;
            case "down":
                float yNegative = player.transform.localPosition.y - 1.0f;
                player.transform.localPosition = new Vector3(player.transform.localPosition.x, yNegative, 0);
                break;
            case "right":
                float xPositive = player.transform.localPosition.x + 1.0f;
                player.transform.localPosition = new Vector3(xPositive, player.transform.localPosition.y, 0);
                break;
            case "left":
                float xNegative = player.transform.localPosition.x - 1.0f;
                player.transform.localPosition = new Vector3(xNegative, player.transform.localPosition.y, 0);
                break;
            case "none":
                Debug.Log("There's a wall there, not walkable.");
                break;

        }
        isPlayerMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // This checks if the player has touched a wall, if does, "teleports" the player to its last position, as cannon be crossed. This is done this way because physics and the rigidbody moves the player to a float position after colliding, breaking the logic afterwards as movement relies on integer vectors
        if (collision.tag == "Wall")
        {
            player.transform.position = new Vector3(_lastKnownPlayerPosition.x, _lastKnownPlayerPosition.y, 0);
        }
    }
}
