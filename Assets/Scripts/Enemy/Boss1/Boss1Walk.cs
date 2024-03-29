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

        public bool willJump;
        public bool willCharge;
        public float after;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            rb = animator.GetComponent<Rigidbody2D>();
            boss = animator.GetComponent<Boss1>();
            attackRanges = boss.GetAttackRanges();
            walkTime = boss.walkTimeRange.GetRange();
            _timer = 0;

            willJump = boss.WillJump();
            willCharge = boss.WillCharge();
            after = Random.Range(0f, 2.5f);

            boss.currentAttack = null;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss.LookAtPlayer();
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, boss.walkSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            _timer += Time.deltaTime;

            if (willCharge || willJump)
            {
                if (_timer >= after)
                {
                    if (willCharge)
                    {
                        if (boss.rageMode && Utils.Utils.Chances(.5f))
                        {
                            boss.currentAttack = boss.attack5;
                            animator.SetTrigger(boss.attack5.trigger);
                            return;
                        }

                        animator.SetTrigger("charge");
                        return;
                    }

                    if (willJump)
                    {
                        animator.SetTrigger("jump");
                        return;
                    }
                }
            }


            if (_timer >= walkTime && !boss.rageMode)
            {
                animator.SetTrigger("idle");
                return;
            }

            if (_timer >= walkTime && boss.rageMode)
            {
                animator.SetTrigger("ready");
                return;
            }

            if (boss.distanceBetweenPlayer >= 10 && boss.rageMode)
            {
                animator.SetTrigger("charge");
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
    }
}