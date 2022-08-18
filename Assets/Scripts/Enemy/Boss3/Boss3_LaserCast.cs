using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Enemy.Boss3
{
    public class Boss3_LaserCast : StateMachineBehaviour
    {
        private Boss3 boss;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss = animator.GetComponent<Boss3>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // boss.LookAtPlayer();

        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            LaserArea laserArea = animator.transform.GetChild(0).GetComponent<LaserArea>();
            laserArea.transform.GetChild(0).gameObject.SetActive(true);
            // boss.isAttacking = false;
            animator.ResetAllTriggers();
        }


    }
}


