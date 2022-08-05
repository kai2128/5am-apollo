using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Class;
namespace Enemy.Boss3
{
    public class Boss3 : Enemy
    {
        public Transform player;
        // public GameObject laserArea;
        public bool isFlipped = false;
        public float distanceBetweenPlayer;

        public float moveSpeed = 10f;

        public bool rageMode = false;

        public Attack melee = new Attack("Melee", 4f, 80);
        public Attack laser = new Attack("Laser", 20f, 20);

        public Attack[] attacks;

        public Attack currentAttack;
        // Start is called before the first frame update

        public class Attack
        {
            public string trigger;
            public float attackRange;
            public int weight;
            public Attack(string trigger, float attackRange, int weight)
            {
                this.trigger = trigger;
                this.attackRange = attackRange;
                this.weight = weight;
            }
        }
        void Awake()
        {
            attacks = new[] { melee, laser };
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

        public float[] GetAttackRanges()
        {
            return attacks.Select(attack => attack.attackRange).ToArray();
        }
        public override void GetHit(AttackArguments getHitBy)
        {

        }
    }

}
