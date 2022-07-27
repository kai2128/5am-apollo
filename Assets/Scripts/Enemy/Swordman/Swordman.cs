using System;
using System.Diagnostics;
using System.Numerics;
using Class;
using DG.Tweening;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;
using Range = Class.Range;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Enemy.Swordman
{
    public class Swordman : Enemy
    {
        private BoxCollider2D attackCollider;
        private BoxCollider2D bodyCollider;
        public LayerMask enemyMask;
        public LayerMask playerMask;

        public State currentState;
        public float timer;
        public Range blockRange = new(.8f, 2f);
        public float blockTimer;
        public float distanceBetweenPlayer;
        public AttackArguments currentAttack;
        public AttackArguments lightAttack = new AttackArguments(15f, 4f);
        private float attackRange = 1.5f;
        public float attackCooldownTime = .3f;
        public bool attackCooldown = false;

        public AttackArguments dashAttack = new AttackArguments(25f, 0f);
        public float dashAttackCooldownTime = 4.5f;
        public bool dashCooldown = false;

        public float runSpeed = 2f;
        public float dashForce = 15f;
        public bool foundPlayer = false;
        public float idleTimer = 1f;

        public enum State
        {
            Run,
            Idle,
            Attack,
            DashAttack,
            Death,
            Block
        }


        void Awake()
        {
            attackCollider = GetComponentInChildren<BoxCollider2D>();
            bodyCollider = GetComponent<BoxCollider2D>();
            currentState = State.Idle;
            enemyXp = 20;
            maxHp = 15;
            currentHp = maxHp;
        }

        public override void GetHit(AttackArguments getHitBy)
        {
            if (currentState is State.Block or State.Death)
                return;

            bool getHitFromBehind = getHitBy.facing == transform.GetFacingFloat();
            sr.BlinkWhite();
            
            // double damage if hit from behind
            currentHp -= getHitFromBehind ? getHitBy.damage * 2 : getHitBy.damage; 
            rb.velocity = getHitBy.PushBackwardForce(transform);
            if (currentState is State.Idle && currentState != State.Death)
            {
                SwitchState(State.Block);
            }


            // can kill directly if attack from behind without being noticed
            if (currentHp <= 0 || (getHitFromBehind && !foundPlayer))
                SwitchState(State.Death);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (!foundPlayer)
                {
                    foundPlayer = true;
                    SwitchState(State.Run);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            distanceBetweenPlayer = Vector2.Distance(transform.position, PlayerManager.Instance.transform.position);
            switch (currentState)
            {
                case State.Run:
                    UpdateRunState();
                    break;
                case State.Idle:
                    UpdateIdleState();
                    break;
                case State.Attack:
                    UpdateAttackState();
                    break;
                case State.DashAttack:
                    UpdateDashAttackState();
                    break;
                case State.Death:
                    UpdateDeathState();
                    break;
                case State.Block:
                    UpdateBlockState();
                    break;
            }
        }

        private void EnterIdleState()
        {
            anim.Play("Idle");
        }

        private void UpdateIdleState()
        {
            if (timer >= idleTimer && foundPlayer)
                SwitchState(State.Run);
        }

        private void ExitIdleState()
        {
            idleTimer = 0.3f;
        }

        private void EnterRunState()
        {
            anim.Play("Run");
        }

        private void UpdateRunState()
        {
            transform.LookAtTarget(PlayerManager.Instance.transform);
            rb.velocity = new Vector2(transform.GetFacingFloat() * runSpeed, 0);

            if (!dashCooldown && distanceBetweenPlayer >= attackRange + 3f)
            {
                SwitchState(State.DashAttack);
                return;
            }

            if (distanceBetweenPlayer <= attackRange )
            {
                if (!attackCooldown)
                    SwitchState(State.Attack);
            }

            if (distanceBetweenPlayer >= chaseRange)
            {
                rb.MovePosition(PlayerManager.Instance.rb.position);
            }
        }

        private void ExitRunState()
        {
        }

        public void Attack()
        {
            Vector2 pos = transform.position;
            pos += transform.GetFacingDirection() * .5f;
            Collider2D col = Physics2D.OverlapCircle(pos, .75f, playerMask);
            if (col != null)
                col.GetComponent<PlayerOnHit>().GetHit(currentAttack.UpdateTransform(transform));
        }

        void OnDrawGizmos()
        {
            Vector2 pos = transform.position;
            pos += transform.GetFacingDirection() * .5f;
            Gizmos.DrawWireSphere(pos, .75f);
        }


        private void EnterAttackState()
        {
            if (attackCooldown)
                SwitchState(State.Run);

            anim.Play("Attack1");
            rb.velocity = new Vector2(transform.GetFacingFloat() * 2.5f, 0);
            currentAttack = lightAttack;
            attackCooldown = true;
            DOVirtual.DelayedCall(attackCooldownTime, () => { attackCooldown = false; });
        }

        private void UpdateAttackState()
        {
            transform.LookAtTarget(PlayerManager.Instance.transform);
            if (anim.HasPlayedOver())
            {
                SwitchState(State.Idle);
            }
        }

        private void ExitAttackState()
        {
            idleTimer = .7f;
        }

        public float chaseRange = 15f;

        private bool dashed = false;

        private void EnterDashAttackState()
        {
            if (dashCooldown)
                SwitchState(State.Block);

            anim.Play("AttackDash");
            transform.LookAtTarget(PlayerManager.Instance.transform);
            currentAttack = dashAttack;
            dashCooldown = true;
            DOVirtual.DelayedCall(dashAttackCooldownTime, () => { dashCooldown = false; });
            rb.velocity = Vector2.zero;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player") && currentState == State.DashAttack)
            {
                Physics2D.IgnoreCollision(bodyCollider, col.collider);
                col.gameObject.GetComponent<PlayerOnHit>().GetHit(currentAttack.UpdateTransform(transform));
            }
        }

        private void UpdateDashAttackState()
        {
            if (anim.HasPlayedOver(.4f) && !dashed)
            {
                dashed = true;
                rb.velocity = new Vector2(transform.GetFacingFloat() * dashForce, 0);
            }

            if (anim.HasPlayedOver())
            {
                SwitchState(State.Idle);
            }
        }

        private void ExitDashAttackState()
        {
            dashed = false;
            rb.velocity = Vector2.zero;
            idleTimer = 1.5f;
            Physics2D.IgnoreCollision(bodyCollider, PlayerManager.Instance.col, false);
        }

        private void EnterDeathState()
        {
            anim.Play("Death");
            anim.enabled = true;
            DOVirtual.DelayedCall(1f, () =>
            {
                DropExperience();
                Destroy(gameObject);
            });
        }

        private void UpdateDeathState()
        {
            anim.enabled = true;
        }

        private void ExitDeathState()
        {
        }

        private void EnterBlockState()
        {
            anim.Play("Block");
            blockTimer = blockRange.GetRange();
            rb.velocity = Vector2.zero;
            DOVirtual.DelayedCall(0.2f, () => { anim.enabled = false; });
        }

        private void UpdateBlockState()
        {
            transform.LookAtTarget(PlayerManager.Instance.transform);
            if (timer >= blockTimer)
            {
                SwitchState(State.Run);
            }
        }

        private void ExitBlockState()
        {
            anim.enabled = true;
        }

        private void SwitchState(State state)
        {
            switch (currentState)
            {
                case State.Run:
                    ExitRunState();
                    break;
                case State.Idle:
                    ExitIdleState();
                    break;
                case State.Attack:
                    ExitAttackState();
                    break;
                case State.DashAttack:
                    ExitDashAttackState();
                    break;
                case State.Death:
                    ExitDeathState();
                    break;
                case State.Block:
                    ExitBlockState();
                    break;
            }

            switch (state)
            {
                case State.Run:
                    EnterRunState();
                    break;
                case State.Idle:
                    EnterIdleState();
                    break;
                case State.Attack:
                    EnterAttackState();
                    break;
                case State.DashAttack:
                    EnterDashAttackState();
                    break;
                case State.Death:
                    EnterDeathState();
                    break;
                case State.Block:
                    EnterBlockState();
                    break;
            }

            currentState = state;
            timer = 0;
        }
    }
}