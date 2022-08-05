using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using System.Linq;
namespace Enemy.Boss3
{
    public class Boss3_move : StateMachineBehaviour
    {
        Transform player;
        Rigidbody2D rb;
        Boss3 boss;

        public float attackRange = 4f;


        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            rb = animator.GetComponent<Rigidbody2D>();
            boss = animator.GetComponent<Boss3>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss.LookAtPlayer();
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, boss.moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            // attackRange = 4f;
            if (boss.distanceBetweenPlayer <= boss.GetAttackRanges().Min())
            {
                animator.SetTrigger("Ready");
            }

        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetAllTriggers();

        }


    }

}
