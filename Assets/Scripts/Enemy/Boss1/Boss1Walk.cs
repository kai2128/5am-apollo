using System.Linq;
using Player;
using UnityEngine;
using Utils;

namespace Enemy.Boss1
{
    public class Boss1Walk : StateMachineBehaviour
    {
        private Transform player;
        private Rigidbody2D rb;
        private Boss1 boss;
        private float _timer;

        public float[] attackRanges;
        public float walkTime;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            rb = animator.GetComponent<Rigidbody2D>();
            boss = animator.GetComponent<Boss1>();
            attackRanges = boss.GetAttackRanges();
            walkTime = boss.walkTimeRange.GetRange();
            _timer = 0;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss.LookAtPlayer();
            Vector2 target = new Vector2(player.position.x,  rb.position.y);
            Vector2 newPos = Vector2.MoveTowards( rb.position, target, boss.walkSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            _timer += Time.deltaTime;

            // if (_timer >= walkTime)
            // {
            //     animator.SetTrigger("idle");
            // }

            if (boss.distanceBetweenPlayer <= attackRanges.Min())
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
    }
}
