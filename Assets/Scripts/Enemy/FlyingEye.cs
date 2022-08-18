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
        private Boss3.Boss3 boss;
        public GameObject launchArmPoint;
        public GameObject target;


        // Start is called before the first frame update
        void Awake()
        {
            launchArmPoint = GameObject.FindGameObjectWithTag("SpawnProjectilePoint");

            target = GameObject.FindGameObjectWithTag("Player");
            maxHp = 15;
            enemyXp = 20;
            currentHp = maxHp;
            currentState = State.Idle;
            boss = GameObject.Find("Boss_3").GetComponent<Boss3.Boss3>();
            if (boss.isEnlarge)
            {
                foundPlayer = true;
            }
            Physics2D.IgnoreCollision(boss.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Physics.IgnoreCollision(boss.GetComponent<Collider>(), GetComponent<Collider>());
            }
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
            if (boss.isEnlarge)
            {
                Destroy(gameObject, 8f);
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
            if (foundPlayer)
            {
                SwitchState(State.Move);
            }
            else
            {
                anim.Play("flying");
                idleTime = Random.Range(2, 5);
            }

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

            if (Utils.Utils.Chances(1f))
            {
                FlipDirection();
            }
        }

        private void UpdateMoveState()
        {


            if (foundPlayer && !boss.isEnlarge)
            {
                transform.LookAtTarget(PlayerManager.Instance.transform);
                Vector3 originalPos = transform.position;
                float posX = originalPos.x;
                float targetX = PlayerManager.Instance.transform.position.x;
                float dist = targetX - posX;
                float nextX = Mathf.MoveTowards(transform.position.x, PlayerManager.Instance.transform.position.x, moveSpeed * Time.deltaTime);
                float baseY = Mathf.Lerp(originalPos.y, PlayerManager.Instance.transform.position.y, (nextX - posX) / dist);
                Vector3 movePosition = new Vector3(nextX, baseY, transform.position.z);
                transform.position = movePosition;

                if (Vector2.Distance(transform.position, PlayerManager.Instance.transform.position) <= attackRange && !attackCooldown)
                {
                    SwitchState(State.Attack);
                }
            }
            else if (foundPlayer && boss.isEnlarge)
            {
                float launchArmPointX = launchArmPoint.transform.position.x;
                float targetX = target.transform.position.x;
                float dist = targetX - launchArmPointX;
                float nextX = Mathf.MoveTowards(transform.position.x, target.transform.position.x, moveSpeed * Time.deltaTime);
                float baseY = Mathf.Lerp(launchArmPoint.transform.position.y, target.transform.position.y, (nextX - launchArmPointX) / dist);
                // height = 2 * (nextX - launchArmPointX) * (nextX - targetX) / (-0.25f * dist * dist);
                Vector3 movePosition = new Vector3(nextX, baseY, transform.position.z);
                transform.position = movePosition;
                transform.LookAtTarget(PlayerManager.Instance.transform);
                if (Vector2.Distance(transform.position, PlayerManager.Instance.transform.position) <= attackRange && !attackCooldown)
                {
                    SwitchState(State.Attack);
                }
            }
            else
            {
                rb.velocity = new Vector2(transform.GetFacingFloat() * moveSpeed, 0);
            }

            if (timer >= 1 && !foundPlayer)
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


