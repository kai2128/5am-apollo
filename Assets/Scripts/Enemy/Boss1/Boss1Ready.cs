using UnityEngine;
using Utils;

namespace Enemy.Boss1
{
    public class Boss1Ready : StateMachineBehaviour
    {
        private Transform player;
        private Boss1 boss;
        private float _timer;
        private Boss1.Attack selectedAttack;
        private float readyTime;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss = animator.GetComponent<Boss1>();
            selectedAttack = boss.GetAttack();
            boss.currentAttack = selectedAttack;
            readyTime = selectedAttack != null ? selectedAttack.readyRange.GetRange() : 0;
            _timer = 0;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (selectedAttack == null)
            {
                animator.SetTrigger("walk");
                return;
            }

            _timer += Time.deltaTime;
            if (_timer >= readyTime)
            {
                animator.SetTrigger(selectedAttack.trigger);
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
