using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Enemy.Boss3
{
    public class Boss3_Wake : StateMachineBehaviour
    {
        private Boss3 boss;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss = animator.GetComponent<Boss3>();

        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Debug.Log(boss.transform.localScale);
            // DOVirtual.Vector3(boss.transform.localScale, new Vector3(1.01f, 0.98f, 1), 1f, (scale) =>
            // {
            //     boss.transform.localScale = scale;
            // });
        }

    }

}
