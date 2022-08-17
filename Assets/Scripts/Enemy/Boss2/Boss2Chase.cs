using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Player;

namespace Enemy.Boss2
{
    public class Boss2Chase : StateMachineBehaviour
    {
        private Transform player;
        private Rigidbody2D rb;
        private Boss2 boss;
        public float speed = 2.5f;
        public float attackRange = 3f;
        // public bool chase = false;
        // public bool allowChase = false;
        private Transform startingPoint;
        private float timer = 0;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            rb = animator.GetComponent<Rigidbody2D>();
            boss = animator.GetComponent<Boss2>();
            Boss2.Attack attack = boss.attack1;
            boss.rb.velocity = new Vector2(attack.force * boss.GetFacingFloat(), 0);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            timer += Time.deltaTime;
            if (timer >= boss.attack1.attackTime || boss.allowChase == false)
            {
                boss.chase = false;
                animator.SetBool("Chase", false);
                animator.SetBool("Ready", true);
                timer = 0;

            }
            else
            {
                boss.ChasePlayer();
                float distance = Vector2.Distance(rb.position, player.position);
                if (distance <= attackRange && boss.isReady == false)
                {
                    animator.SetTrigger("Attack");
                }
            }

        }



        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // close the trigger so wont keep attack on every updates
            animator.ResetAllTriggers();

        }
    }

}

