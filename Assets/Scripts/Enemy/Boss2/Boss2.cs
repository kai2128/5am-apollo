using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Class;
using DG.Tweening;
using Player;
using static Utils.Utils;
using Utils;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using Range = Class.Range;

namespace Enemy.Boss2
{
    public class Boss2 : Enemy
    {
        public bool isFlipped;
        private Transform player;
        /// private Rigidbody2D rb;
        public float chaseSpeed = 2.5f;
        public float returnSpeed = 5.0f;
        public Range idleTimeRange = new(0.5f, 1f);
        public Range flyTimeRange = new(3f, 5f);
        public float distanceBetweenPlayer;
        public float currentDistanceBetweenPlayer;


        [Header("Attacks")]
        public float readyTime = 0.5f;

        public Attack attack1 = new Attack(15f, 5f, 3f, "Chase", .8f, 60, 2f); // Chase and attack
        public Attack attack2 = new Attack(15f, 2f, 4f, "Fireball", 1.5f, 40, 5f); // Fireball 
        public Attack attack3 = new Attack(15f, 3f, 7f, "UpAndDown", 1.8f, 0, 10f); // Attack up and down
        public Attack[] attacks;

        public class Attack
        {
            public float damage;
            public float force;
            public float range;
            public string trigger;
            public float idlePercentage;
            public int weight;
            public float attackTime;

            public AttackArguments GetAttackArgs()
            {
                return new AttackArguments(damage, force);
            }

            public Attack(float damage, float force, float range, string trigger, float idlePercentage, int weight, float attackTime)
            {
                this.damage = damage;
                this.force = force;
                this.range = range;
                this.trigger = trigger;
                this.idlePercentage = idlePercentage;
                this.weight = weight;
                this.attackTime = attackTime;
            }
        }
        public Attack currentAttack;

        [Header("States")]
        public float recoverTime = 3f;
        public float tenacity;
        public float maxTenacity = 50f;
        public bool isReady = false;
        public bool readyAttack = false;
        public bool rageMode = false;
        public bool dead = false;

        //ground check
        [Header("GroundCheck")]

        public LayerMask groundLayer;
        private bool goingUp = true;
        private bool facingLeft = true;

        public Transform[] patrolPoint;
        public Transform startingPoint;
        public Transform[] attackPoint;
        public bool chase;
        public bool allowChase;
        public float attackUDSpeed = 2.5f;
        public Vector2 attackUDDirection;

        private PolygonCollider2D bossCollider;

        // Start is called before the first frame update
        private void Awake()
        {
            attacks = new[] { attack1, attack2, attack3 };
            maxHp = 120;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            currentHp = maxHp;
            tenacity = maxTenacity;
            enemyName = "Flying Talbot";
            attackUDDirection.Normalize();
            bossCollider = transform.GetComponent<PolygonCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            distanceBetweenPlayer = Vector2.Distance(player.position, rb.position);
        }


        // Boss RageMode = Attack Up and Down trigger 
        public void EnterRageMode()
        {
            rageMode = true;
            sr.color = new Color(1f, .8f, .7f);


            attack1.weight = -0;
            attack2.weight = -0;
            attack3.weight = 100;
            Debug.Log("Enter RageMode");
        }

        private void ExitRageMode()
        {
            rageMode = false;
            sr.color = Color.white;

            attack1.weight = 70;
            attack2.weight = 30;
            attack3.weight = 0;
        }

        public Attack GetAttack()
        {
            int sum = 0;
            int rand = Random.Range(0, 100);
            foreach (var atk in attacks)
            {
                sum += atk.weight;
                if (rand <= sum)
                {
                    return atk;
                }
            }
            return null;
        }


        public float[] GetAttackRanges()
        {
            return attacks.Select(attack => attack.range).ToArray();
        }

        public AttackArguments GetAttackArgs(Attack attack)
        {
            return attack.GetAttackArgs().UpdateTransform(transform);
        }

        public float GetFacingFloat()
        {
            return isFlipped ? -1 : 1;
        }

        //attacked by player
        public override void GetHit(AttackArguments getHitBy)
        {
            if (dead)
                return;

            ReduceTenacity(getHitBy); // still able to reduce tenacity if attack from behind
            // cannot damage boss if attack from behind
            if (getHitBy.facing == GetFacingFloat())
                return;
            float damage = getHitBy.damage;
            // reduce 50% damage if getting hit in ready state
            if (isReady)
                damage *= .5f;

            currentHp -= damage;
            sr.BlinkWhite();
            if (rageMode)
            {
                sr.material.color = Color.red;
            }

            if (currentHp / maxHp < .4 && !rageMode)
            {

                EnterRageMode();
            }


            if (currentHp <= 0)
            {
                dead = true;
                anim.Play("Die");
            }
        }

        private void ReduceTenacity(AttackArguments atkArgs)
        {
            tenacity -= atkArgs.damage;
        }



        //when attack player == boss collide with player
        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                var attackArgs = GetAttackArgs(currentAttack);
                col.gameObject.GetComponent<PlayerOnHit>().GetHit(attackArgs);
            }
        }

        void ChangeDirection()
        {
            goingUp = !goingUp;
            // transform.Rotate(180f, 0f, 0f); (wrong)
            //transform.position = new Vector2(transform.position.x, -transform.position.y);
            attackUDDirection.y *= -1;
        }

        void Flip()
        {
            facingLeft = !facingLeft;
            attackUDDirection.x *= -1;
            transform.Rotate(0f, 180f, 0f);

        }

        // public void LookAtPlayer()
        // {
        //     Vector3 flipped = transform.localScale;
        //     flipped.z *= -1f;

        //     if (transform.position.x < player.position.x && isFlipped)
        //     {
        //         transform.localScale = flipped;
        //         transform.Rotate(0f, 180f, 0f);
        //         isFlipped = false;
        //     }
        //     else if (transform.position.x > player.position.x && !isFlipped)
        //     {
        //         transform.localScale = flipped;
        //         transform.Rotate(0f, 180f, 0f);
        //         isFlipped = true;
        //     }
        // }
        // public void LookAtPoint()
        // {

        //     if (transform.position.x < startingPoint.position.x && isFlipped)
        //     {
        //         transform.rotation = Quaternion.Euler(0, 0, 0);
        //     }
        //     else
        //     {
        //         transform.rotation = Quaternion.Euler(0, 180, 0);
        //     }

        // }

        public void ReturnStartPosition()
        {
            transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, returnSpeed * Time.fixedDeltaTime);
        }

        public void ChasePlayer()
        {
            if (chase == true && allowChase == true)
            {
                // if (isTouchingUp && goingUp)
                // {
                //     ChangeDirection();
                // }
                // else if (isTouchingDown && !goingUp)
                // {
                //     ChangeDirection();
                // }

                // if (isTouchingWall)
                // {
                //     if (facingLeft)
                //     {
                //         Flip();
                //     }
                //     else if (!facingLeft)
                //     {
                //         Flip();
                //     }
                // }


                // LookAtPlayer();
                // move towards player
                Vector2 newPos = Vector2.MoveTowards(rb.position, player.transform.position, chaseSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
            else
            {
                // LookAtPoint();
                ReturnStartPosition();
            }
        }

        public void Ready()
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, rb.position, chaseSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }

        public void SpawnFireball()
        {

        }

        // ------------------------ Ground Check ------------------------
        public bool isGround()
        {
            float extraHeightText = 0.1f;
            RaycastHit2D raycastHit = Physics2D.Raycast(bossCollider.bounds.center, Vector2.down, bossCollider.bounds.extents.y + extraHeightText, groundLayer);
            Color rayColor;
            if (raycastHit.collider != null)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(bossCollider.bounds.center, Vector2.down * (bossCollider.bounds.extents.y + extraHeightText), rayColor);

            return raycastHit.collider != null;
        }

        public bool isTop()
        {
            float extraHeightText = 0.1f;
            RaycastHit2D raycastHit = Physics2D.Raycast(bossCollider.bounds.center, Vector2.up, bossCollider.bounds.extents.y + extraHeightText, groundLayer);
            Color rayColor;
            if (raycastHit.collider != null)
            {
                rayColor = Color.cyan;
            }
            else
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(bossCollider.bounds.center, Vector2.up * (bossCollider.bounds.extents.y + extraHeightText), rayColor);

            return raycastHit.collider != null;
        }

        public bool isWall()
        {
            float extraHeightText = 0.1f;
            RaycastHit2D raycastHit = Physics2D.Raycast(bossCollider.bounds.center, Vector2.right, bossCollider.bounds.extents.x + extraHeightText, groundLayer);
            Color rayColor;
            if (raycastHit.collider != null)
            {
                rayColor = Color.yellow;
            }
            else
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(bossCollider.bounds.center, Vector2.right * (bossCollider.bounds.extents.x + extraHeightText), rayColor);

            return raycastHit.collider != null;
        }

        public void Die()
        {
            sr.BlinkWhite();
            DropExperience();
            Destroy(gameObject);
        }

        public void ResetBoss()
        {
            transform.position = startingPosition;
            currentHp = maxHp;
            tenacity = maxTenacity;
            ExitRageMode();
            anim.Rebind();
            anim.Update(0f);
            anim.enabled = false;
        }
    }

}