using DG.Tweening;
using UnityEngine;

namespace Enemy.Boss1
{
    public class Boss1Stun : StateMachineBehaviour
    {
        private Boss1 boss;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss = animator.GetComponent<Boss1>();
            DOVirtual.Float(boss.tenacity, boss.maxTenacity, boss.recoverTime, recovered =>
            {
                boss.tenacity = recovered;
            });
        }
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

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
