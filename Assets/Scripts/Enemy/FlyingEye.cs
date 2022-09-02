using System;
using Class;
using DG.Tweening;
using Player;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;
using Pathfinding;
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
        public float moveSpeed = 100f;
        public bool foundPlayer = false;
        public float attackRange = 1.5f;
        public bool attackCooldown;
        public bool isDead = false;
        public float attackCooldownTime = 1f;
        public AttackArguments atkArgs = new AttackArguments(10f, 5f);
        private Boss3.Boss3 boss;
        public GameObject launchArmPoint;
        public GameObject target;

        //pathfinding 
        public float nextWaypointDistance = 5f; //how close the enemy needs to be to a waypoint before move to the next one

        Path path; //current path following
        int currentWayPoint = 0; //current waypoint along the path that we are following
        bool reachedEndOfPath = false;

        Seeker seeker;

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
            seeker = GetComponent<Seeker>();
            InvokeRepeating("UpdatePath", 0f, .5f); //update path instantly every half second;

        }
        void UpdatePath()
        {
            if (seeker.IsDone())
            {
                // make sure that it isnt currently calculating a path
                seeker.StartPath(rb.position, PlayerManager.Instance.transform.position, OnPathComplete);
            }
        }

        void OnPathComplete(Path p)
        {
            if (!p.error) //if didnt get error
            {
                path = p;
                currentWayPoint = 0;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Physics.IgnoreCollision(boss.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }
        // Update is called once per frame
        void FixedUpdate()
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
            if (boss.isdead)
            {
                Destroy(gameObject);
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
            if (foundPlayer && path == null)
                return;
            if (currentWayPoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }
            if (foundPlayer)
            {
                transform.LookAtTarget(PlayerManager.Instance.transform);

                // float dist = (transform.position - PlayerManager.Instance.transform.position).sqrMagnitude;
                // transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

                //follow along the path
                Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
                Vector2 force = direction * moveSpeed * Time.deltaTime;
                rb.AddForce(force);
                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
                if (distance < nextWaypointDistance)
                {
                    currentWayPoint++;
                }
                if (Vector2.Distance(rb.position, PlayerManager.Instance.transform.position) <= attackRange && !attackCooldown)
                {
                    SwitchState(State.Attack);
                }
            }
            else
            {
                rb.velocity = new Vector2(transform.GetFacingFloat() * moveSpeed * Time.deltaTime, 0);
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
            transform.LookAtTarget(PlayerManager.Instance.transform);
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


