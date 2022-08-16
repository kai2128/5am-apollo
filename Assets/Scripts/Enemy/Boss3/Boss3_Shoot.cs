using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace Enemy.Boss3
{
    public class Boss3_Shoot : StateMachineBehaviour
    {
        private Boss3 boss;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // animator.SetInteger("shootCount", animator.GetInteger("shootCount") - 1);
            boss = animator.GetComponent<Boss3>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss.isAttacking = false;
            animator.ResetAllTriggers();
        }

    }

}
