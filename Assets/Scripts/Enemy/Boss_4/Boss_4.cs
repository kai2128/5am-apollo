using System;
using Class;
using DG.Tweening;
using Player;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Boss_4 : Enemy
    {
        public enum State
        {
            Idle,
            Attack,
            Hurt,
            Die,
            Run,
            Cast
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

        private bool transparent = false;
        public float minTimeBeforeNextTransparent = 1f;
        public float maxTimeBeforeNextTransparent = 8f;
        private float transparentInterval = 1f;

        public Transform spawnPoint;
        private float anger = 0;
        public GameObject go;
        public GameObject go2;
        public bool attack2 = false;
        public AttackArguments atkArgs = new AttackArguments(30f, 5f);

        void Awake()
        {
            maxHp = 100;
            enemyXp = 100;
            currentHp = maxHp;
            enemyName = "Bringer of Deadth";
            currentState = State.Idle;
            go = GameObject.Find("Effect");
            go2 = GameObject.Find("Effect2");
            go.SetActive(false);
            go2.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            //ChasePlayer();
            HandleTransparentSprite();
            //sr.material.color = Color.clear;

            if(go2.activeSelf)
            {
                PlayerManager.Instance.playerMovement.PauseMovement(3f); //stop the player movement, how?
            }

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

        private void HandleTransparentSprite()
        {
            transparentInterval -= Time.deltaTime;
            if (transparentInterval < 0f)
            {
                if (!transparent)
                {
                    transparentInterval = 1f;
                    DOVirtual.Float(1, 0, .3f, (opacity) => { sr.material.color = new Color(1f, 1f, 1f, opacity); });
                    transparent = true;
                }
                else
                {
                    transparentInterval = Random.Range(minTimeBeforeNextTransparent, maxTimeBeforeNextTransparent);
                    sr.material.color = new Color(1f, 1f, 1f, 1f);
                    transparent = false;
                }
            }
        }

        // public void ChasePlayer()
        // {
        //     Vector2 target = new Vector2(PlayerManager.Instance.transform.position.x, rb.position.y);
        //     Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
        //     rb.MovePosition(newPos);

        //     if (Vector2.Distance(PlayerManager.Instance.transform.position, rb.position) <= attackRange)
        //     {
        //         SwitchState(State.Attack);
        //         foundPlayer = true;
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
                if(PlayerManager.Instance.anim.enabled) //if player die stop attacking
                {
                    go.SetActive(true);
                    attack2 = true;
                }

                SwitchState(State.Run);
            }
        }

        private void ExitIdleState()
        {
        }

        private void EnterRunState()
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

        private void UpdateRunState()
        {
            rb.velocity = new Vector2(transform.GetFacingFloat() * moveSpeed, 0);

            if (foundPlayer)
            {
                if (Vector2.Distance(transform.position, PlayerManager.Instance.transform.position) <= attackRange && !attackCooldown)
                {
                    attack2 = false;
                    go.SetActive(false);
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
            if (!PlayerManager.Instance.anim.enabled)
            {
                SwitchState(State.Idle);
                foundPlayer = false;
                go2.SetActive(false);
            }

            if (anger == 100)
            {
                anim.Play("Cast");
                go2.SetActive(true);
                attack2 = true;
                anger = 0; //only cast once
            }
            else
            {
                anim.Play("Attack");
                go2.SetActive(false);
            }
        }

        private void UpdateAttackState()
        {
            if (anim.HasPlayedOver())
            {
                attack2 = false;
                go2.SetActive(false);
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
            if (anger <= 80)
            {
                anger += 20;
                //sr.BlinkWhite();

                if (anger >= 50)
                {
                    sr.BlinkRed();
                }
            }
            anim.Play("Hit");
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

        public void ResetBoss()
        {
            transform.localPosition = spawnPoint.localPosition;
            currentHp = maxHp;
            anim.Rebind();
            anim.Update(0f);
            anim.enabled = false;
        }

    }
}

