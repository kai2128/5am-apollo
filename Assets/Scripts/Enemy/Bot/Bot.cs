using System;
using Class;
using DG.Tweening;
using Player;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Enemy
{

    public class Bot : Enemy
    {
        public enum State
        {
            Idle, Move, Attack, Hit, Die
        }

        public State currentState;
        public float timer;
        public float idleTime;
        public float moveSpeed = 5;
        public bool foundPlayer = false;
        public float attackRange = 2f;
        public bool attackCooldown;
        public bool isDead = false;
        public float attackCooldownTime = 3f;
        private bool hitted = false;
        public AttackArguments atkArgs = new AttackArguments(13f, 5f); // damage and distance

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
            hitted = true;
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
                // update distance after get hit
                col.gameObject.GetComponent<PlayerOnHit>().GetHit(atkArgs.UpdateTransform(transform));

        }


        private void EnterIdleState()
        {
            anim.Play(stateName: "Idle");
            idleTime = Random.Range(2, 5);
        }

        private void UpdateIdleState()
        {
            //change state when get hit
            // if (timer >= idleTime)
            // {
            //     SwitchState(State.Move);
            // }
            if (hitted)
            {
                SwitchState(State.Move);
            }
        }

        private void ExitIdleState()
        {

        }

        private void EnterMoveState()
        {
            anim.Play(stateName: "Move");

            // foundPlayer = true;
            // transform.LookAtTarget(PlayerManager.Instance.transform);
            // // print("found player");

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

        private void EnterHitState()
        {
            anim.Play(stateName: "Hit");
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


        private void EnterAttackState()
        {
            anim.Play(stateName: "Attack");
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

        private void EnterDieState()
        {
            anim.Play(stateName: "Die");
        }

        private void UpdateDieState()
        {

        }

        private void ExitDieState()
        {

        }
        public void Die()
        {
            sr.BlinkWhite();
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