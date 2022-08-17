using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Boss2
{
    public class Boss2Attack : StateMachineBehaviour
    {
        private Boss2 boss;
        private Boss2.Attack selectedAttack;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss = animator.GetComponent<Boss2>();
            selectedAttack = boss.GetAttack();

        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (selectedAttack == boss.attack1)
            {
                boss.readyTime = 1f;
                boss.chase = false;
                boss.isReady = true;
                animator.ResetTrigger("Attack");
                animator.SetBool("Chase", false);
                animator.SetBool("Ready", true);
                Debug.Log("boss REady" + boss.isReady);
            }
            else if (selectedAttack == boss.attack3)
            {
                animator.ResetTrigger("Attack");
                animator.SetBool("UpAndDown", true);
                animator.SetBool("Ready", false);
                boss.isReady = true;

            }
        }
    }
}
