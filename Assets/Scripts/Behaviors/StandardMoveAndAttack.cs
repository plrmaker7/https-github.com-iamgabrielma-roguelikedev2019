using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardMoveAndAttack : IBehavior
{
    // As this inherits from Interface IBehavior, is a contract, and needs to implement the interface member Act()
    public bool Act(Entity entity) {

        /* - A monster should perform a standard melee attack on the player if the player is adjacent to the monster.
         *       
         */

        Debug.Log(entity.name + " is thinking about life");

        return true;
    }
}
