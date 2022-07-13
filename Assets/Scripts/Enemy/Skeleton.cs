using System;
using System.Collections;
using Class;
using DG.Tweening;
using Player;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Skeleton : Enemy
    {
        private enum State
        {
            Walk,
            Idle,
            Hit,
            Chase,
            React,
            Attack,
            Die,
        }

        public float getHitForce;
        private Vector2 _dir;

        [SerializeField] private State currentState;
        [Header("Properties")] public float attackDamage = 20f;
        public float attackRange;
        public float walkSpeed = 1.5f;
        public float idleTime = 1.5f;
        public float walkTime = 5f;
        [Header("Roam")] public float roamRadius = 20f;
        public float minRoamDistance = 4f;
        public float maxRoamDistance = 10f;

        public LayerMask groundLayer;
        [SerializeField]private float timer;
        [SerializeField] private Transform groundCheck, wallCheck;
        [SerializeField] private float groundCheckDist, wallCheckDist;
        [SerializeField] bool groundDetected, wallDetected;

        private Vector2 movement;
        private new void Start()
        {
            base.Start();
            currentState = State.Idle;
        }

        private AttackArguments getHitArgs;

        public override void GetHit(AttackArguments atkArgs)
        {
            getHitArgs = atkArgs;
            SwitchState(State.Hit);
        }

        private void Update()
        {
            switch (currentState)
            {
                case State.Walk:
                    UpdateWalkingState();
                    break;
                case State.Idle:
                    UpdateIdleState();
                    break;
                case State.Hit:
                    UpdateHitState();
                    break;
                case State.Chase:
                    UpdateChaseState();
                    break;
                case State.React:
                    UpdateReactState();
                    break;
                case State.Attack:
                    UpdateAttackState();
                    break;
                case State.Die:
                    UpdateDieState();
                    break;
            }
        }
        

        private void OnDrawGizmos()
        {
            var groundPos = groundCheck.position;
            Gizmos.DrawLine(groundPos, new Vector2(groundPos.x, groundPos.y - groundCheckDist));
            var wallPos = wallCheck.position;
            Gizmos.DrawLine(wallPos, new Vector2(wallPos.x + wallCheckDist, wallPos.y));

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x  + attackRange, transform.position.y));
        }

        public void OnFrontCollisionEnter(Collider2D col)
        {
            if (col.CompareTag("Ground"))
            {
                Debug.Log(1);
            }
        }

        private void EnterWalkingState()
        {
            anim.SetBool("isWalking", true);
        }

        private void UpdateWalkingState()
        {
            groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDist, groundLayer);
            wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDist, groundLayer);

            if (!groundDetected || wallDetected)
            {
                FlipDirection();
            }
            else
            {
                movement.Set(walkSpeed * transform.GetFacingDirection().x, rb.velocity.y);
                rb.velocity = movement;
            }

            timer += Time.deltaTime;
            if (timer > walkTime)
            {
                SwitchState(State.Idle);
            }
        }

        private void ExitWalkingState()
        {
            anim.SetBool("isWalking", false);
        }

        private void EnterHitState()
        {
            transform.LookAtTarget(getHitArgs.transform);
            getHitForce = getHitArgs.force;
            _dir = transform.GetOppositeDirection();
            anim.SetTrigger("hit");
            DOVirtual.Float(.2f, 1f, 0.4f, duration => anim.speed = duration);
        }

        private void UpdateHitState()
        {
            animInfo = anim.GetCurrentAnimatorStateInfo(0);
            rb.velocity = _dir * getHitForce;
            if (animInfo.normalizedTime >= 1f)
                SwitchState(State.React);
        }

        private void ExitHitState()
        {
        }

        private void EnterIdleState()
        {
        }

        private void UpdateIdleState()
        {
            timer += Time.deltaTime;
            if (timer > idleTime)
            {
                SwitchState(State.Walk);
            }
        }

        private void ExitIdleState()
        {
        }

        private void EnterAttackState()
        {
            anim.SetTrigger("attack");
            rb.velocity = Vector2.zero;
        }

        private void UpdateAttackState()
        {
            if(anim.HasPlayedOver(1f))
                SwitchState(State.Chase);   
        }

        private void ExitAttackState()
        {
        }

        private void EnterChaseState()
        {
            anim.SetBool("isWalking", true);
        }

        private void UpdateChaseState()
        {
            var position = rb.position;
            transform.LookAtTarget(PlayerManager.Instance.transform);
            Vector2 target = new Vector2(PlayerManager.Instance.transform.position.x,  position.y);
            Vector2 newPos = Vector2.MoveTowards( position, target, walkSpeed * 1.2f * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            if (Vector2.Distance(target, rb.position) <= attackRange)
            {
                SwitchState(State.Attack); 
            }
        }

        private void ExitChaseState()
        {
            anim.SetBool("isWalking", false);
        }

        private void EnterReactState()
        {
            anim.SetTrigger("react");
        }

        private void UpdateReactState()
        {
            Debug.Log(anim.HasPlayedOver());
            if (anim.HasPlayedOver())
            {
                SwitchState(State.Chase);
            }
        }

        private void ExitReactState()
        {
        }

        private void EnterDieState()
        {
            anim.SetTrigger("die");
        }

        private void UpdateDieState()
        {
            if (anim.HasPlayedOver(1f))
            {
                Destroy(this);                
            }
        }

        private void ExitDieState()
        {
        }

        private void SwitchState(State state)
        {
            switch (currentState)
            {
                case State.Walk:
                    ExitWalkingState();
                    break;
                case State.Idle:
                    ExitIdleState();
                    break;
                case State.Hit:
                    ExitHitState();
                    break;
                case State.Chase:
                    ExitChaseState();
                    break;
                case State.React:
                    ExitReactState();
                    break;
                case State.Attack:
                    ExitAttackState();
                    break;
                case State.Die:
                    ExitDieState();
                    break;
            }

            switch (state)
            {
                case State.Walk:
                    EnterWalkingState();
                    break;
                case State.Idle:
                    EnterIdleState();
                    break;
                case State.Hit:
                    EnterHitState();
                    break;
                case State.Chase:
                    EnterChaseState();
                    break;
                case State.React:
                    EnterReactState();
                    break;
                case State.Attack:
                    EnterAttackState();
                    break;
                case State.Die:
                    EnterDieState();
                    break;
            }

            timer = 0;
            currentState = state;
        }
    }
}