using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Utils;

namespace Enemy.Boss2
{
    public class Boss2UpAndDown : StateMachineBehaviour
    {
        private Transform player;
        private Boss2 boss;
        private Boss2.Attack attack;
        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private Transform[] targetPoint;
        private int target;
        public float attackRange = 3f;
        private int index;
        private int index2;
        //private Vector2 newPos;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            boss = animator.GetComponent<Boss2>();
            rb = animator.GetComponent<Rigidbody2D>();
            sr = animator.GetComponent<SpriteRenderer>();
            attack = boss.attack3;
            targetPoint = boss.attackPoint;
            target = 0;
            index = -1;
            sr.material.color = Color.red;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (boss.isGround() || boss.isWall())
            {
                index++;

                // random generate a top point
                target = Random.Range(0, 5);
                //int[] counter = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
                // int[] counter = new int[] { 0, 8, 1, 9, 2, 10, 3, 11, 4, 12, 5, 13, 6, 14, 7, 15 };
                // if (index >= 16)
                // {
                //     index = 0;
                // }
                // target = counter[index];
            }
            else if (boss.isTop())
            {
                index++;
                //random generate a bottom point
                target = Random.Range(6, 11);
                //int[] counter = new int[] { 8, 9, 10, 11, 12, 13, 14, 15 };
                // int[] counter = new int[] { 0, 8, 1, 9, 2, 10, 3, 11, 4, 12, 5, 13, 6, 14, 7, 15 };
                // if (index2 >= 7)
                // {
                //     index2 = 0;
                // } 
                // if (index >= 16)
                // {
                //     index = 1;
                // }
                // target = counter[index];
            }
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetPoint[target].position, boss.attackUDSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            float distance = Vector2.Distance(rb.position, player.position);
            if (distance <= attackRange && boss.isReady == false)
            {
                animator.SetTrigger("Attack");

            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

    }

}
