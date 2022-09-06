using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Enemy.Boss2
{
    public class Boss2Fly : StateMachineBehaviour
    {
        private Transform player;
        private Rigidbody2D rb;
        private Boss2 boss;
        private float flyTimer;

        private Transform[] flyingPoint;
        public float speed = 2.5f;
        private int randomPoint;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            rb = animator.GetComponent<Rigidbody2D>();
            boss = animator.GetComponent<Boss2>();
            flyingPoint = boss.patrolPoint;
            flyTimer = 0f;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            flyTimer += Time.deltaTime;
            if (flyTimer == 0f)
            {
                randomPoint = Random.Range(0, flyingPoint.Length);
            }
            if (flyTimer >= 3f)
            {
                randomPoint = Random.Range(0, flyingPoint.Length);
                flyTimer = 0f;
            }

            if (boss.readyAttack == true && boss.isReady == false)
            {
                animator.SetBool("Fly", false);
                animator.SetBool("Ready", true);
            }
            else
            {

                Vector2 newPos = Vector2.MoveTowards(rb.position, flyingPoint[randomPoint].position, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }

}

