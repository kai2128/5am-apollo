using System;
using Class;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Player;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EvilWizard : Enemy
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
        public float moveSpeed = 3;
        public bool foundPlayer = false;
        public float attackRange = 2f;
        public bool attackCooldown;
        public bool isDead = false;
        public float attackCooldownTime = 2f;
        public AttackArguments atkArgs = new AttackArguments(10f, 15f);

        void Awake()
        {
            maxHp = 50;
            enemyXp = 40;
            currentHp = maxHp;
            currentState = State.Idle;
        }

        // Update is called once per frame
        void Update()
        {
            if(currentState != State.Attack)
                ChasePlayer();

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

        public void ChasePlayer()
        {
            Vector2 target = new Vector2(PlayerManager.Instance.transform.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (!attackCooldown && Vector2.Distance(PlayerManager.Instance.transform.position, rb.position) <= attackRange)
            {
                SwitchState(State.Attack);
                foundPlayer = true;
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

        private Tweener pauseMovementTween;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (pauseMovementTween != null)
                {
                    pauseMovementTween.Kill();
                }

                col.gameObject.GetComponent<PlayerOnHit>().GetHit(atkArgs.UpdateTransform(transform));
                pauseMovementTween = DOVirtual.Vector3(Vector2.zero, Vector2.zero, 1f, (x) => { PlayerManager.Instance.rb.velocity = x; });
                // PlayerManager.Instance.rb.velocity.DOMove(Vector2.zero, 5);
                // StartCoroutine(PlayerManager.Instance.playerMovement.PauseMovement(5));
            }
        }

         private void EnterIdleState()
        {
            anim.Play("Idle");
            idleTime = Random.Range(2, 5);
        }
        
        private void UpdateIdleState()
        {
            if (timer >= idleTime)
            {
                SwitchState(State.Move);
            }
        }
        
        private void ExitIdleState()
        {
        }

        private void EnterMoveState()
        {
            anim.Play("Move");
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

        private void EnterAttackState()
        {
            anim.Play("Attack");
            //sr.color = Color.white;
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
            anim.Play("Hit");
            //sr.BlinkWhite();
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

        private void EnterDieState()
        {
            anim.Play("Die");
        }

        private void UpdateDieState()
        {
        }

        private void ExitDieState()
        {
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

