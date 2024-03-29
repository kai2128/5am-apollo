using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using System.Linq;
namespace Enemy.Boss3
{
    public class Boss3_Immune : StateMachineBehaviour
    {
        private Boss3 boss;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss = animator.GetComponent<Boss3>();

        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (boss.goingEnlarge)
            {
                animator.SetTrigger("Enlarge");

            }
            // boss.isImmune = true;
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // animator.ResetAllTriggers();
            boss.isImmune = false;
        }


    }

}
