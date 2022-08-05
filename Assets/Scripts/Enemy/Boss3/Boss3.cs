using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Class;
using Player;
using static Utils.Utils;
using Utils;
namespace Enemy.Boss3
{
    public class Boss3 : Enemy
    {
        public new string name;
        public Transform player;
        // public GameObject laserArea;
        public bool isFlipped = false;
        public float distanceBetweenPlayer;

        public float moveSpeed = 10f;

        public bool rageMode = false;
        public bool isImmune = false;
        public Attack melee = new Attack(10f, 1f, "Melee", 1.2f, 80);
        public Attack laser = new Attack(50f, 1f, "Laser", 20f, 20);

        public Attack[] attacks;

        public Attack currentAttack;
        // Start is called before the first frame update

        public class Attack
        {
            public float damage;
            public float force;
            public string trigger;
            public float attackRange;
            public int weight;
            public Attack(float damage, float force, string trigger, float attackRange, int weight)
            {
                this.damage = damage;
                this.force = force;
                this.trigger = trigger;
                this.attackRange = attackRange;
                this.weight = weight;
            }

            public AttackArguments GetAttackArgs()
            {
                return new AttackArguments(damage, force);
            }
        }
        void Awake()
        {
            attacks = new[] { melee, laser };
            name = "Mecha Golem";
            currentHp = maxHp;
        }

        // Update is called once per frame
        void Update()
        {
            distanceBetweenPlayer = Vector2.Distance(player.transform.position, rb.position);

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
            }
            else if (transform.position.x > player.position.x && !isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = true;
            }

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

        public AttackArguments GetAttackArgs(Attack attack)
        {
            return attack.GetAttackArgs().UpdateTransform(transform);
        }

        public float[] GetAttackRanges()
        {
            return attacks.Select(attack => attack.attackRange).ToArray();
        }
        public override void GetHit(AttackArguments getHitBy)
        {
            if (isImmune)
            {
                return;
            }
            float damage = getHitBy.damage;
            currentHp -= damage;
            sr.BlinkWhite();

            if (currentHp <= 0)
            {
                anim.SetBool("isImmune", true);

                rageMode = true;
                isImmune = true;
            }
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                var attackArgs = GetAttackArgs(currentAttack);
                col.gameObject.GetComponent<PlayerOnHit>().GetHit(attackArgs);
            }
        }

        public void StopEnlarge()
        {
            transform.localScale = new Vector3(4, 3, 1);
            Debug.Log(transform.localScale);
            isImmune = false;
            EnterRageMode();
        }

        public void EnterRageMode()
        {
            maxHp = 500;
            currentHp = 500;
        }
    }

}
