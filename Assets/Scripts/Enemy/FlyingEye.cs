using System;
using Class;
using DG.Tweening;
using Player;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class FlyingEye : Enemy
    {
        public enum State
        {
            Idle,
            Attack,
            Hit,
            Die,
            Move
        }

        public State currentState;
        public float timer;
        public float idleTime;
        public float moveSpeed = 5;
        public bool foundPlayer = false;
        public float attackRange = 2f;
        public bool attackCooldown;
        public bool isDead = false;
        public float attackCooldownTime = 1f;
        public AttackArguments atkArgs = new AttackArguments(10f, 5f);

        // Start is called before the first frame update
        void Awake()
        {
            maxHp = 15;
            enemyXp = 20;
            currentHp = maxHp;
            currentState = State.Idle;
        }

        // Update is called once per frame
        void Update()
        {

            timer += Time.deltaTime;
            switch (currentState)
            {
                case State.Move:
                    UpdateMoveState();
                    break;
                case State.Idle:
                    UpdateIdleState();
                    break;
                case State.Hit:
                    UpdateHitState();
                    break;
                case State.Attack:
                    UpdateAttackState();
                    break;
                case State.Die:
                    UpdateDieState();
                    break;
            }
        }


        public override void GetHit(AttackArguments getHitBy)
        {
            currentHp -= getHitBy.damage;

            if (currentHp <= 0)
            {
                isDead = true;
                SwitchState(State.Die);
                return;
            }

            SwitchState(State.Hit);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
                col.gameObject.GetComponent<PlayerOnHit>().GetHit(atkArgs.UpdateTransform(transform));
        }

        private void EnterIdleState()
        {
            anim.Play("flying");
            idleTime = Random.Range(2, 5);
        }

        private void UpdateIdleState()
        {
            if (timer >= idleTime && Vector2.Distance(transform.position, PlayerManager.Instance.transform.position) > attackRange)
            {
                SwitchState(State.Move);
            }
        }

        private void ExitIdleState()
        {
        }

        private void EnterMoveState()
        {
            anim.Play("flying");

            if (foundPlayer)
            {
                transform.LookAtTarget(PlayerManager.Instance.transform);

                return;
            }

            if (Utils.Utils.Chances(.5f))
            {
                FlipDirection();
            }
        }

        private void UpdateMoveState()
        {
            rb.velocity = new Vector2(transform.GetFacingFloat() * moveSpeed, 0);

            if (foundPlayer)
            {
                if (Vector2.Distance(transform.position, PlayerManager.Instance.transform.position) <= attackRange && !attackCooldown)
                {
                    SwitchState(State.Attack);
                }
            }

            if (timer >= 3)
            {
                SwitchState(State.Idle);
            }
        }

        private void ExitMoveState()
        {
        }

        private void EnterDieState()
        {
            anim.Play("death");

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

        private void EnterAttackState()
        {
            anim.Play("attack");
        }

        private void UpdateAttackState()
        {
            if (anim.HasPlayedOver())
            {
                attackCooldown = true;
                SwitchState(State.Move);
            }
        }

        private void ExitAttackState()
        {
            DOVirtual.DelayedCall(attackCooldownTime, () => attackCooldown = false);
        }

        private void EnterHitState()
        {
            anim.Play("takehit");
            sr.BlinkWhite();
        }

        private void UpdateHitState()
        {
            if (anim.HasPlayedOver())
            {
                SwitchState(State.Move);
            }
        }

        private void ExitHitState()
        {
            foundPlayer = true;
        }

        public void Die()
        {
            DropExperience();
            Destroy(gameObject);
        }

        private void SwitchState(State state)
        {
            switch (currentState)
            {
                case State.Move:
                    ExitMoveState();
                    break;
                case State.Idle:
                    ExitIdleState();
                    break;
                case State.Hit:
                    ExitHitState();
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
                case State.Move:
                    EnterMoveState();
                    break;
                case State.Idle:
                    EnterIdleState();
                    break;
                case State.Hit:
                    EnterHitState();
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


