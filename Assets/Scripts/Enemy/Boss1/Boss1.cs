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

namespace Enemy.Boss1
{
    public class Boss1 : Enemy
    {
        [Serializable]
        public class Range
        {
            public float min;
            public float max;

            public float GetRange()
            {
                return Random.Range(min, max);
            }
            public Range(float min, float max)
            {
                this.min = min;
                this.max = max;
            }
        }
        
        public bool isFlipped;
        private Transform player;
        public Transform spawnPoint;

        [Header("Properties")]
        public float jumpForce = 8f;
        public float walkSpeed = 1.5f;
        public float chargeSpeed = 3f;
        public float chargeTime = 3f;
        public Range idleTimeRange =  new(0.5f, 1f);
        public Range walkTimeRange =  new(3f, 5f);
        public float distanceBetweenPlayer;
        public float currentDistanceBetweenPlayer;
        
        public new string name;
        [Header("Attacks")]
        public float readyTime = 1f;

        // public float attack1Damage = 20f;
        // public float attack1Force = 1f;
        // public float attack1Range = 3f;
        //
        // public float attack2Damage = 25f;
        // public float attack2Force = 2f;
        // public float attack2Range = 3f;
        //
        // public float attack3Damage = 25f;
        // public float attack3Force = 3f;
        // public float attack3Range = 3f;
        //
        // public float attack4Damage = 25f;
        // public float attack4Force = 15f;
        // public float attack4Range = 3f;
        //
        // public float attack5Damage = 30f;
        // public float attack5Force = 30f;
        // public float attack5Range = 3f;

        public Attack attack1 = new Attack(20f, 1f, 3f, "attack_1",.8f, 35); // slash
        public Attack attack2 = new Attack(25f, 2f, 3f, "attack_2",1.5f, 25); // double slash
        public Attack attack3 = new Attack(25f, 3f, 3f, "attack_3",1.8f, 20); // ranged attack
        public Attack attack4 = new Attack(25f, 3.5f, 3f, "attack_4",2.2f,  10); // ground attack
        public Attack attack5 = new Attack(15f, 1f, 8f, "attack_5",1.3f, 0); // dash attack

        public Attack[] attacks;
        
        [Serializable]
        public class Attack
        {
            public float damage;
            public float force;
            public float range;
            public string trigger;
            public float idlePercentage;
            public int weight;

            public AttackArguments GetAttackArgs()
            {
                return new AttackArguments(damage, force);
            }
            
            public Attack(float damage, float force, float range, string trigger, float idlePercentage, int weight)
            {
                this.damage = damage;
                this.force = force;
                this.range = range;
                this.trigger = trigger;
                this.idlePercentage = idlePercentage;
                this.weight = weight;
            }
        }

        public Attack currentAttack;
        
        
        [Header("States")]
        public float recoverTime = 3f;
        public float tenacity;
        public float maxTenacity = 50f;
        public bool isStunned = false;
        public bool isReady = false;
        public bool rageMode = false;
        public bool dead = false;
        // Start is called before the first frame update
        private void Awake()
        {
            attacks = new[]{attack1, attack2, attack3, attack4, attack5};
            player = GameObject.FindGameObjectWithTag("Player").transform;
            currentHp = maxHp;
            tenacity = maxTenacity;
            name = "Meta Knight";
        }

        public void EnterRageMode()
        {
            rageMode = true;
            sr.color = new Color(1f, .5f, .5f);
            currentHp += maxHp * .2f;
            
            walkSpeed *= 1.2f;
            chargeSpeed *= 1.2f;
            attack1.damage *= 1.2f;
            attack2.damage *= 1.2f;
            attack3.damage *= 1.2f;
            attack4.damage *= 1.2f;
            attack5.damage *= 1.2f;
            
            attack1.weight = 15;
            attack2.weight = 25;
            attack3.weight = 25;
            attack4.weight = 20;
            attack5.weight = 15;
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

        public bool WillJump()
        {
            if (distanceBetweenPlayer >= 10f && PlayerManager.Instance.rb.velocity.y != 0)
            {
                return Chances(.7f);
            }
            return Chances(.2f);
        }

        public bool WillCharge()
        {
            if (distanceBetweenPlayer >= 15f && rageMode)
            {
                return Chances(.8f);
            }
            return Chances(.05f);
        }

        public float GetFacingFloat()
        {
            return isFlipped ? -1 : 1;
        }
        
        public override void GetHit(AttackArguments atkArgs)
        {
            if(dead)
                return;
            
            ReduceTenacity(atkArgs); // still able to reduce tenacity if attack from behind
            // cannot damage boss if attack from behind
            if (atkArgs.facing == GetFacingFloat())
                return;

            // reduce 50% damage if getting hit in ready state
            if (isReady)
                atkArgs.damage /= .5f;
            
            currentHp -= atkArgs.damage;
            StartCoroutine(BlinkWhite());

            if (currentHp / maxHp < .4 && !rageMode)
            {
                EnterRageMode();
            }

            if (currentHp <= 0)
            {
                dead = true;
                anim.SetBool("dead", true);
            }
        }
        private void ReduceTenacity(AttackArguments atkArgs)
        {
            tenacity -= atkArgs.damage;
        }
        
        private IEnumerator BlinkWhite()
        {
            Color defaultColor = sr.material.color;
            sr.material.color = new Color(255, 255, 255);
            yield return new WaitForSeconds(0.2f);
            sr.material.color = defaultColor;
        }

        // Update is called once per frame
        void Update()
        {
            // bind animator
            anim.SetFloat("tenacity", tenacity);
            anim.SetFloat("verticalVelocity", rb.velocity.y);
            distanceBetweenPlayer = Vector2.Distance(player.position, rb.position);
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                var attackArgs = GetAttackArgs(currentAttack);
                col.gameObject.GetComponent<PlayerOnHit>().GetHit(attackArgs);
            }
        }

        public void LookAtPlayer()
        {
            Vector3 flipped = transform.localScale;
            flipped.z *= -1f;

            if (transform.position.x < player.position.x && isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = false;
            } else if (transform.position.x > player.position.x && !isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f,0f);
                isFlipped = true;
            }
        }

        public void ResetBoss()
        {
            transform.localPosition = spawnPoint.localPosition;
            currentHp = maxHp;
            anim.Rebind();
            anim.Update(0f);
            anim.enabled = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPoint.position, 1);
        }
    }
}
