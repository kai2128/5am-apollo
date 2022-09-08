using System;
using Class;
using DG.Tweening;
using Player;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Slime2 : Enemy
    {
        public enum State
        {
            Idle,
            Move,
            Die
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

        public AttackArguments atkArgs = new AttackArguments(10f, 5f);

        void Awake()
        {
            maxHp = 15;
            //enemyXp = 20;
            currentHp = maxHp;
            currentState = State.Idle;
        }

        void Update()
        {
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

            if (Vector2.Distance(PlayerManager.Instance.transform.position, rb.position) <= attackRange)
            {
                SwitchState(State.Die);
            }
        }

        public override void GetHit(AttackArguments getHitBy)
        {
            currentHp -= getHitBy.damage;
            
            SwitchState(State.Die);
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

            if (Utils.Utils.Chances(.5f))
            {
                FlipDirection();
            } 
        }
        
        private void UpdateMoveState()
        {
            rb.velocity = new Vector2(transform.GetFacingFloat() * moveSpeed, 0);
            
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
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player")){
                col.gameObject.GetComponent<PlayerOnHit>().GetHit(atkArgs.UpdateTransform(transform));
                EnterDieState();
            }
                
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
                case State.Die:
                    EnterDieState();
                    break;
            }

            timer = 0;
            currentState = state;
        }
    }
}