using UnityEngine;
using Utils;

namespace Enemy.Boss1
{
    public class Boss1Idle : StateMachineBehaviour
    {
        private Boss1 boss;
        public float idleTime;
        private float _timer;
        
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _timer = 0;
            boss = animator.GetComponent<Boss1>();
            idleTime = boss.idleTimeRange.GetRange();
            if (boss.currentAttack != null)
            {
                idleTime *= boss.currentAttack.idlePercentage;
            }
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _timer += Time.deltaTime;
            if (_timer >= idleTime)
            {
                animator.SetTrigger("walk");
            }
        }

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
