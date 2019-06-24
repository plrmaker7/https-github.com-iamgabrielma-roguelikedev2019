﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    /* InputHandler may be a limited name, as at the moment deals with Player logic as well */


    private bool isPlayerMoving; // This avoids that there's multiple movement until the previous movement hasn't completed. While isPlayerMoving = True, no new movements can start.
    //private bool isPlayerMovementTurn; // Allows/Disallows the Player to perform movement based on the Game State.

    GameObject player; // Self reference, in order to use MovePlayer() easily
    private Vector3 _lastKnownPlayerPosition; // We use this to track players last position before their next move

    public static bool isFOVrecompute; // If True, FOV is recomputed on GridGenerator.cs (temporary, maybe this needs to go into GameMap or something)



    private void Start()
    {
        // TODO: add nullcheck. We assign player to the static __player instance, shouldn't be null but check for safety.
        player = Engine.__player;
        isFOVrecompute = true; // When the player appears for first time, we need to calculate the initial FOV
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
        isFOVrecompute = true; // When a movement is success, then we recompute FOV
        isPlayerMoving = false;

        GameStateManager.__gameTimeTicks++; // Adds a tick to Game Time.

        //GameStateManager.__gameState = GameStateManager.GameState.playerTurn; // Call player turn for turn completed and debug log gametime
        // Once we've moved, is the Enemy turn:
        //Debug.Log("State 1: Setting enemy turn");
        //GameStateManager.__gameState = GameStateManager.GameState.enemyTurn; // OFF.

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // This checks if the player has touched a wall or enemy, if does, "teleports" the player to its last position, as cannon be crossed. This is done this way because physics and the rigidbody moves the player to a float position after colliding, breaking the logic afterwards as movement relies on integer vectors
        // Elements must be "isTrigger" and "Static collider" at the moment for this to work, if changes, still breaks the player position.
        if (collision.tag == "Wall" || collision.tag == "Enemy")
        {
            player.transform.position = new Vector3(_lastKnownPlayerPosition.x, _lastKnownPlayerPosition.y, 0);
        }

        // TODO: Bump attack when the player hits the enemy by hittings its tile
        if (collision.tag == "Enemy")
        {
            //Entity _enemy = new Entity((int)collision.transform.localPosition.x, (int)collision.transform.localPosition.y, "Enemy", _test_npc, new Vector3(_randomVector.x, _randomVector.y, 0)); ;
            Debug.Log("Bump attack: resolving defense!");
            Entity.ResolveDefense(player, collision.gameObject);
            Entity.ResolveDeath(collision.gameObject);

        }
    }
}
