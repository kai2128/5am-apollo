using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Boss2
{
    public class Boss2_Fireball_idle : StateMachineBehaviour
    {

        private Fireball fireball;
        public float lifeTime;


        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            fireball = animator.GetComponent<Fireball>();
            lifeTime = fireball.lifetime;
            animator.SetBool("Hit", true);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                animator.SetBool("Hit", true);
            }

            if (fireball.hit == true)
            {
                animator.SetBool("Hit", true);
            }

        }

        //  OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

    }
}