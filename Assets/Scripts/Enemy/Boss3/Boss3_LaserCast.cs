using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
public class Boss3_LaserCast : StateMachineBehaviour
{
    // public Transform firePoint;
    // private RayCastHit2D hitInfo;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    // public GameObject laser;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        // Debug.Log();
        LaserArea laserArea = animator.transform.GetChild(0).GetComponent<LaserArea>();
        laserArea.transform.GetChild(0).gameObject.SetActive(true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // if (hitInfo)
        // {
        //     Debug.Log(hitInfo.transform.name);
        // }

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
