using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
namespace Enemy.Boss3
{
    public class Boss3_Melee : StateMachineBehaviour
    {
        private Boss3 boss;
        private Rigidbody2D rb;
        public float chargeSpeed = 15f;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            rb = animator.GetComponent<Rigidbody2D>();
            boss = animator.GetComponent<Boss3>();



        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //charge
            boss.LookAtPlayer();
            Vector2 target = new Vector2(boss.player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, chargeSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }


    }

}
