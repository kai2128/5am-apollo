using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using System.Linq;
namespace Enemy.Boss3
{
    public class Boss3_Ready : StateMachineBehaviour
    {
        private Boss3 boss;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss = animator.GetComponent<Boss3>();
            boss.currentAttack = boss.GetAttack();

        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss.LookAtPlayer();
            if (boss.currentAttack == null)
            {
                boss.isAttacking = false;
                if (boss.isEnlarge)
                {
                    Debug.Log("Now is idle");
                    animator.SetTrigger("Idle");
                }
                else
                {
                    animator.SetTrigger("Walk");
                }


            }
            else
            {

                if (boss.currentAttack.trigger != "Sheild")
                {
                    boss.isAttacking = true;
                }
                animator.SetTrigger(boss.currentAttack.trigger);

            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetAllTriggers();
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }

}
