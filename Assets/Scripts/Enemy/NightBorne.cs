using System;
using Class;
using DG.Tweening;
using Player;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class NightBorne : Enemy
    {
        public enum State
        {
            Idle,
            Attack,
            Hurt,
            Die,
            Run
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

        // private bool transparent = false;
        // public float minTimeBeforeNextTransparent = 1f;
        // public float maxTimeBeforeNextTransparent = 5f;
        // private float transparentInterval = 1f; 

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
            timer += Time.deltaTime;
            //HandleTransparentSprite();
            switch (currentState)
            {
                case State.Run:
                    UpdateRunState();
                    break;
                case State.Idle:
                    UpdateIdleState();
                    break;
                case State.Hurt:
                    UpdateHurtState();
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
            
            SwitchState(State.Hurt);
        }

        // testttt
        // private void HandleTransparentSprite() 
        // {
        //     transparentInterval -= Time.deltaTime;
        //     if (transparentInterval < 0f)
        //     {
        //         if (!transparent)
        //         {
        //             transparentInterval = 1f;
        //             GetComponent<SpriteRenderer>().enabled = false;
        //             transparent = true;
        //         }
        //         else
        //         {
        //             transparentInterval = Random.Range(minTimeBeforeNextTransparent, maxTimeBeforeNextTransparent);
        //             GetComponent<SpriteRenderer>().enabled = true;
        //             transparent = false;
        //         }
        //     }
        // }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
                col.gameObject.GetComponent<PlayerOnHit>().GetHit(atkArgs.UpdateTransform(transform));
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
                SwitchState(State.Run);
            }
        }
        
        private void ExitIdleState()
        {
        }

        private void EnterRunState()
        {
            anim.Play("Run");
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

        private void UpdateRunState()
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

        private void ExitRunState()
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
                SwitchState(State.Run);
            }
        }

        private void ExitAttackState()
        {
            DOVirtual.DelayedCall(attackCooldownTime, () => attackCooldown = false);
        }

        private void EnterHurtState()
        {
            anim.Play("Hurt");
            //sr.BlinkWhite();
        }

        private void UpdateHurtState()
        {
            if (anim.HasPlayedOver())
            {
                SwitchState(State.Run);
            }
        }

        private void ExitHurtState()
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
                case State.Run:
                    ExitRunState();
                    break;
                case State.Idle:
                    ExitIdleState();
                    break;
                case State.Hurt:
                    ExitHurtState();
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
                case State.Run:
                    EnterRunState();
                    break;
                case State.Idle:
                    EnterIdleState();
                    break;
                case State.Hurt:
                    EnterHurtState();
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

