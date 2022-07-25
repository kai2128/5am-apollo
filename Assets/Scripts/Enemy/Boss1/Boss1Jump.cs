using System;
using Unity.VisualScripting;
using UnityEngine;
using static Utils.Utils;

namespace Enemy.Boss1
{
    public class Boss1Jump : StateMachineBehaviour
    {
        private Boss1 boss;
        private Rigidbody2D rb;

        public bool willAttack = false;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss = animator.GetComponent<Boss1>();
            rb = animator.GetComponent<Rigidbody2D>();
            float jumpDirection = boss.GetFacingFloat();
            if (boss.rageMode) // jump towards player in rage mode
            {
                jumpDirection = Chances(.8f) ? 1 : -1;
            }else if(boss.tenacity <= 20f) // jump away more if tenacity is low
            {
                jumpDirection = Chances(.7f) ? -1 : 1;
            }
            rb.velocity = new Vector2( jumpDirection * 3f, boss.jumpForce);
            
            // only will attack in mid air in rage mode
            if(boss.rageMode)
                willAttack = Chances(.4f);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss.LookAtPlayer();

            if (willAttack)
            {
                if (rb.velocity.y <= 0.1)
                {
                    boss.currentAttack = boss.attack4;
                    animator.SetTrigger("attack_4");
                }
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

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
