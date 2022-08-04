using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Class;
using Range = Class.Range;
using Utils;
namespace Enemy.Boss3
{
    public class Boss3_Idle : StateMachineBehaviour
    {
        private Boss3 boss;
        public float idleTime;
        private Range idleTimeRange = new(0.5f, 3.0f);
        private float timer;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            timer = 0;
            boss = animator.GetComponent<Boss3>();
            idleTime = 3f;

            if (boss.currentAttack != null)
            {

                idleTime += idleTimeRange.GetRange();
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            timer += Time.deltaTime;
            if (timer >= idleTime)
            {
                animator.SetTrigger("Move");
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetAllTriggers();
        }


    }

}
