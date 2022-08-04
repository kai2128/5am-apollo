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
        public GameObject laserArea;
        public bool isFlipped = false;
        public float distanceBetweenPlayer;

        public Attack melee = new Attack("Melee", 4f);
        public Attack laser = new Attack("Laser", 5f);

        public Attack[] attacks;

        public Attack currentAttack;
        // Start is called before the first frame update

        public class Attack
        {
            public string trigger;
            public float attackRange;
            public Attack(string trigger, float attackRange)
            {
                this.trigger = trigger;
                this.attackRange = attackRange;
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
            return attacks[1];
        }

        public float[] GetAttackRanges()
        {
            return attacks.Select(attack => attack.attackRange).ToArray();
        }
        public override void GetHit(AttackArguments getHitBy)
        {

        }

        // public void LaserEffect()
        // {
        //     Laser laser = laserArea.GetComponentInChildren<Laser>();
        //     laser.gameObject.SetActive(true);
        //     // Debug.Log(laser.gameObject);
        // }


    }

}
