using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Boss2
{
    public class Boss2Ready : StateMachineBehaviour
    {
        private Transform player;
        private Boss2 boss;
        private Rigidbody2D rb;
        private float _timer;
        private Boss2.Attack selectedAttack;
        private float readyTime;

        //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss = animator.GetComponent<Boss2>();
            rb = animator.GetComponent<Rigidbody2D>();
            selectedAttack = boss.GetAttack();
            boss.currentAttack = selectedAttack;
            // readyTime = selectedAttack != null ? boss.readyTime : 0;
            //readyTime = boss.readyTime;
            //readyTime = selectedAttack != null ? boss.readyTime : 0;
            readyTime = 0.5f;
            _timer = 0;
            // boss.isReady = true;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //boss.LookAtPlayer();
            if (selectedAttack == null || boss.readyAttack == false || boss.allowChase == false)
            {
                animator.SetBool("Fly", true);
                animator.SetBool("Ready", false);
                return;
            }

            _timer += Time.deltaTime;
            if (_timer >= readyTime)
            {
                if (selectedAttack.trigger == "Chase")
                {
                    if (boss.isReady == true)
                    {
                        boss.isReady = false;
                    }
                    animator.SetBool(selectedAttack.trigger, true);
                    animator.SetBool("Ready", false);
                    boss.chase = true;
                }
                else if (selectedAttack.trigger == "Fireball")
                {
                    animator.SetBool(selectedAttack.trigger, true);
                    animator.SetBool("Ready", false);
                }
                else if (selectedAttack.trigger == "UpAndDown")
                {

                    animator.SetBool(selectedAttack.trigger, true);
                    animator.SetBool("Ready", false);

                }
            }
            else
            {
                animator.Play("Ready");
                boss.Ready();
            }


        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss.isReady = false;
        }

    }

}
