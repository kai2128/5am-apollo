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

namespace Enemy.Boss1
{
    public class Boss1 : Enemy
    {
        public bool isFlipped;
        private Transform player;
        public Transform spawnPoint;

        [HideInInspector] public AttackHint attackHint; 

        [Header("Properties")]
        public float jumpForce = 8f;
        public float walkSpeed = 1.5f;
        public float chargeSpeed = 3f;
        public float chargeTime = 3f;
        public Range idleTimeRange =  new(0.5f, 1f);
        public Range walkTimeRange =  new(3f, 5f);
        public float distanceBetweenPlayer;
        public float currentDistanceBetweenPlayer;
        [Header("Attacks")]
        public float readyTime = 1f;

        public Attack attack1 = new Attack(20f, 1f, 3f, "attack_1",.8f, 60, AttackHint.AttackType.Normal); // slash
        public Attack attack2 = new Attack(25f, 2f, 4f, "attack_2",1.5f, 25, AttackHint.AttackType.Medium); // double slash
        public Attack attack3 = new Attack(25f, 3f, 7f, "attack_3",1.8f, 100, AttackHint.AttackType.Danger); // ranged attack
        public Attack attack4 = new Attack(25f, 3.5f, 6f, "attack_4",2.2f,  0, AttackHint.AttackType.Special); // ground attack
        public Attack attack5 = new Attack(15f, 1f, 8f, "attack_5",1.3f, 0, AttackHint.AttackType.Danger); // dash attack

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
            public AttackHint.AttackType hint;

            public AttackArguments GetAttackArgs()
            {
                return new AttackArguments(damage, force);
            }
            
            public Attack(float damage, float force, float range, string trigger, float idlePercentage, int weight, AttackHint.AttackType hint)
            {
                this.damage = damage;
                this.force = force;
                this.range = range;
                this.trigger = trigger;
                this.idlePercentage = idlePercentage;
                this.weight = weight;
                this.hint = hint;
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
            enemyName = "Meta Knight";
            enemyXp = 100;
            attackHint = GetComponentInChildren<AttackHint>(true);
        }

        public void EnterRageMode()
        {
            rageMode = true;
            sr.color = new Color(1f, .8f, .7f);
            idleTimeRange.max = 0.7f;
            idleTimeRange.min = 0.1f;
            currentHp += maxHp * .3f;
            
            walkSpeed *= 1.3f;
            chargeSpeed *= 1.4f;
            attack1.damage *= 1.2f;
            attack2.damage *= 1.2f;
            attack3.damage *= 1.2f;
            attack4.damage *= 1.2f;
            attack5.damage *= 1.2f;
            
            attack1.weight = 5;
            attack2.weight = 20;
            attack3.weight = 30;
            attack4.weight = 25;
            attack5.weight = 20;
        }

        private void ExitRageMode()
        {
            rageMode = false;
            sr.color = Color.white;
            idleTimeRange = new Range(.5f, 1f);
            
            walkSpeed /= 1.3f;
            chargeSpeed /= 1.4f;
            attack1.damage /= 1.2f;
            attack2.damage /= 1.2f;
            attack3.damage /= 1.2f;
            attack4.damage /= 1.2f;
            attack5.damage /= 1.2f;
            
            attack1.weight = 35;
            attack2.weight = 25;
            attack3.weight = 20;
            attack4.weight = 10;
            attack5.weight = 0;
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
                    print(sum);
                    print(rand);
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
            if (distanceBetweenPlayer >= 7f && PlayerManager.Instance.rb.velocity.y != 0)
            {
                return Chances(.6f);
            }
            return Chances(.2f);
        }

        public bool WillCharge()
        {
            if (distanceBetweenPlayer >= 7f && rageMode)
            {
                return Chances(.7f);
            }
            return Chances(.1f);
        }

        public float GetFacingFloat()
        {
            return isFlipped ? -1 : 1;
        }
        
        public override void GetHit(AttackArguments getHitBy)
        {
            if(dead)
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

            if (currentHp / maxHp < .4 && !rageMode)
            {
                EnterRageMode();
            }

            if (currentHp <= 0)
            {
                dead = true;
                anim.Play("Die");
                PlayerManager.Instance.SetStatusMessage("You have defeated Meta Knight!");
                DropExperience();
                if (!PlayerManager.Instance.unlockedSword)
                {
                    DOVirtual.DelayedCall(2f, () =>
                    {
                        PlayerManager.Instance.SetStatusMessage("Unlocked 'Sword', press 'Q' to switch weapon.");
                    });
                    PlayerManager.Instance.unlockedSword = true;                    
                }                
            }
        }
        private void ReduceTenacity(AttackArguments atkArgs)
        {
            float rate = 0.08f;
            if (atkArgs.damage > 15)
                rate += 0.02f;
            if (tenacity < 20f)
                rate += 0.08f;
            if (rageMode)
                rate -= 0.05f;
            tenacity -= maxTenacity * rate;
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
            tenacity = maxTenacity;
            ExitRageMode();
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
