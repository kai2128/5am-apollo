using System;
using System.Collections;
using System.Diagnostics;
using System.Numerics;
using Class;
using DG.Tweening;
using Player;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

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
        private PolygonCollider2D weaponCol;

        [SerializeField] private State currentState;
        [Header("Properties")] public float attackDamage = 20f;
        public float attackRange;
        public float walkSpeed = 1.5f;
        public float idleTime = 1.5f;
        public float walkTime = 5f;
        [Header("Roam")] public float chaseRadius = 15f;
        public float walkRadius = 10f;

        public LayerMask playerLayer;
        public LayerMask groundLayer;
        [SerializeField] private float timer;
        [SerializeField] private Transform groundCheck, wallCheck;
        [SerializeField] private float groundCheckDist, wallCheckDist;
        [SerializeField] bool groundDetected, wallDetected;
        // LevelSystem playerLevel;
        private Vector2 movement;

        private new void Start()
        {
            base.Start();
            maxHp = 20;
            currentHp = maxHp;
            enemyXp = 30;
            weaponCol = GetComponent<PolygonCollider2D>();
            currentState = State.Idle;
        }

        private AttackArguments getHitArgs;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (currentState == State.Attack)
                {
                    if (weaponCol.IsTouching(col) && col.CompareTag("Player"))
                        col.gameObject.GetComponent<PlayerOnHit>().GetHit(new AttackArguments(attackDamage));
                }
                if (currentState is State.Idle or State.Walk)
                {
                    SwitchState(State.React);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (Vector2.Distance(col.gameObject.transform.position, startingPosition) >= chaseRadius)
                return;

            if (col.CompareTag("Player"))
            {
                if (currentState == State.Idle || currentState == State.Walk)
                {
                    SwitchState(State.React);
                }
            }
        }

        public override void GetHit(AttackArguments atkArgs)
        {
            if (currentState == State.Die)
                return;

            // while attacking cannot be cancelled / damaged
            if (currentState == State.Attack)
            {
                // only can attack from behind if skeleton is attacking
                if (atkArgs.dir.x != transform.GetFacingDirection().x)
                    return;
            }

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
            Start();

            // terrain check
            var groundPos = groundCheck.position;
            Gizmos.DrawLine(groundPos, new Vector2(groundPos.x, groundPos.y - groundCheckDist));
            var wallPos = wallCheck.position;
            Gizmos.DrawLine(wallPos, new Vector2(wallPos.x + wallCheckDist, wallPos.y));

            // attack range
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + attackRange, transform.position.y));

            // chase range
            var radius = transform.position;
            radius.x -= chaseRadius;
            Gizmos.DrawLine(radius, new Vector2(radius.x + chaseRadius * 2, startingPosition.y));

            Gizmos.color = Color.grey;
            // walk range
            radius = transform.position;
            radius.x -= walkRadius;
            Gizmos.DrawLine(radius, new Vector2(radius.x + walkRadius * 2, startingPosition.y));
        }

        private void EnterWalkingState()
        {
            anim.Play("Walk");
        }

        private void UpdateWalkingState()
        {
            groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDist, groundLayer);
            wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDist, groundLayer);

            if (!groundDetected || wallDetected)
            {
                FlipDirection();
            }
            else if (Vector2.Distance(startingPosition, rb.position) >= chaseRadius)
            {
                transform.LookAtTarget(startingPosition);
                movement.Set(walkSpeed * transform.GetFacingDirection().x, rb.velocity.y);
                rb.velocity = movement;
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
        }

        private void EnterHitState()
        {
            currentHp -= getHitArgs.damage;
            transform.LookAtTarget(getHitArgs.transform);
            getHitForce = getHitArgs.force;
            _dir = transform.GetOppositeDirection();
            anim.Play("Hit");
            DOVirtual.Float(.2f, 1f, 0.4f, duration => anim.speed = duration);
        }

        private void UpdateHitState()
        {
            rb.velocity = _dir * getHitForce;
            timer += Time.deltaTime;
            if (currentHp <= 0)
            {
                SwitchState(State.Die);
            }
            if (timer > 0.3f)
            {
                if (anim.HasPlayedOver(0.9f))
                {
                    SwitchState(State.React);
                }
            }
        }

        private void ExitHitState()
        {

        }

        private void EnterIdleState()
        {
            anim.Play("Idle");
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
            anim.Play("Attack");
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
            sr.color = new Color(1f, .8f, .8f);
        }

        private void UpdateAttackState()
        {
            if (anim.HasPlayedOver(1f))
                SwitchState(State.Chase);
        }

        private void ExitAttackState()
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            anim.Play("Idle");
            sr.color = Color.white;
        }

        private void EnterChaseState()
        {
            anim.Play("Walk");
            sr.color = new Color(1f, .8f, .8f);
        }

        private void UpdateChaseState()
        {
            groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDist, groundLayer);
            wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDist, groundLayer);
            var position = rb.position;
            transform.LookAtTarget(PlayerManager.Instance.transform);
            Vector2 target = new Vector2(PlayerManager.Instance.transform.position.x, position.y);
            Vector2 newPos = Vector2.MoveTowards(position, target, walkSpeed * 1.2f * Time.fixedDeltaTime);
            if (groundDetected && !wallDetected)
                rb.MovePosition(newPos);

            if (Vector2.Distance(target, rb.position) <= attackRange)
            {
                SwitchState(State.Attack);
            }

            if (Vector2.Distance(target, startingPosition) >= chaseRadius)
            {
                if (!transform.IsFacingTarget(startingPosition))
                {
                    SwitchState(State.Idle);
                }
            }
        }

        private void ExitChaseState()
        {
            sr.color = Color.white;
        }

        private void EnterReactState()
        {
            transform.LookAtTarget(PlayerManager.Instance.transform);
            anim.Play("React");
        }

        private void UpdateReactState()
        {
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
            anim.Play("Die");
        }

        private void UpdateDieState()
        {
            if (anim.HasPlayedOver())
            {
                Destroy(gameObject);
                DropExperience();
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