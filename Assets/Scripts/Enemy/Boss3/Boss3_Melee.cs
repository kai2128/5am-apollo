using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Utils;

namespace Enemy.Boss3
{
    public class Boss3_Melee : StateMachineBehaviour
    {
        private Boss3 boss;
        private Rigidbody2D rb;
        private SpriteRenderer sr;
        public float chargeSpeed = 15f;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            rb = animator.GetComponent<Rigidbody2D>();
            boss = animator.GetComponent<Boss3>();
            sr = animator.GetComponent<SpriteRenderer>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //charge to player if player far away
            if (boss.distanceBetweenPlayer > 1.2f)
            {
                boss.LookAtPlayer();
                Vector2 target = new Vector2(boss.player.position.x, rb.position.y);
                Vector2 newPos = Vector2.MoveTowards(rb.position, target, chargeSpeed * Time.fixedDeltaTime);
                //change to red color when charge
                sr.color = new Color(255, 0, 0, 255); // (r,g,b,alpha)
                rb.MovePosition(newPos);
            }

        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetAllTriggers();
            sr.color = new Color(255, 255, 255, 255);
        }


    }

}
