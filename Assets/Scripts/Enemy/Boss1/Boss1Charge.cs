using System.Diagnostics;
using System.Linq;
using Player;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;

namespace Enemy.Boss1
{
    public class Boss1Charge : StateMachineBehaviour
    {
        private Boss1 boss;
        public float _timer;
        public float maxChargeTime;
        private Rigidbody2D rb;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _timer = 0;
            boss = animator.GetComponent<Boss1>();
            rb = animator.GetComponent<Rigidbody2D>();
            maxChargeTime = boss.chargeTime;
            Debug.Log(boss.GetAttackRanges().Min());
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _timer += Time.deltaTime;
            boss.LookAtPlayer();
            Vector2 target = new Vector2(PlayerManager.Instance.rb.position.x,  rb.position.y);
            Vector2 newPos = Vector2.MoveTowards( rb.position, target, boss.chargeSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (_timer >= maxChargeTime)
            {
                animator.SetTrigger("walk");
                return;
            }

            if (boss.distanceBetweenPlayer <= boss.GetAttackRanges().Min())
            {
                animator.SetTrigger("ready");
                boss.currentDistanceBetweenPlayer = boss.distanceBetweenPlayer;
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.ResetAllTriggers();
        }

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
