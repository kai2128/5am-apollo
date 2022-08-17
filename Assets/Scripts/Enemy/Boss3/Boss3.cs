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
        [Header("Boss3 Common")]

        public Transform player;
        // public GameObject laserArea;
        public bool isFlipped = false;
        public float distanceBetweenPlayer;
        public float moveSpeed = 10f;

        public float armour = 0;

        public bool isImmune = false;
        public Attack melee = new Attack(10f, 1f, "Melee", 2f, 80);
        public Attack laser = new Attack(30f, 1f, "Laser", 20f, 20);
        public Attack shoot = new Attack(20f, 1f, "Shoot", 20f, 20);
        public Attack shield_cast = new Attack(0f, 1f, "Sheild", 0f, 20);

        public Attack[] attacks;
        public bool isdead = false;
        public bool canFlip = true;
        public Attack currentAttack;
        [Header("Mini Boss")]
        public Attack[] miniBossAttacks;


        [Header("Giant Boss")]
        public Attack[] giantBossAttacks;

        public Boss3LeftShoulder leftShoulder;
        public Boss3RightShoulder rightShoulder;
        public Boss3Head head;

        public bool isAttackedHead;
        public bool isAttackedLeftShoulder;
        public bool isAttackedRightShoulder;

        public float weaknessRadius;
        public bool rageMode = false;
        public bool isEnlarge = false;

        public bool goingEnlarge = false;
        public bool isAttacking = false;
        public float SpawnProjectilesCooldown = 10f;
        public int NumberOfSpawns = 0;
        public float timer = 0f;
        // Projectiele
        public ProjectileBehavior ProjectilePrefab;
        public BombProjectileBehavior bombPrefab;
        public Transform LaunchArmProjectileOffset;
        public Transform ProjectileSpawnPoint1;
        public Transform ProjectileSpawnPoint2;
        public Transform ProjectileSpawnPoint3;
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

            public void SetWeight(int weight)
            {
                this.weight = weight;
            }
        }
        void Awake()
        {
            miniBossAttacks = new[] { melee, laser };
            giantBossAttacks = new[] { shoot, shield_cast, laser };
            attacks = miniBossAttacks; // mini boss attacks
            enemyName = "Mecha Golem";
            maxHp = 100;
            currentHp = maxHp;
            enemyXp = 100;

        }

        // Update is called once per frame
        void Update()
        {
            distanceBetweenPlayer = Vector2.Distance(player.transform.position, rb.position);
            if (rageMode && !isdead)
            {
                timer += Time.deltaTime;
                if (distanceBetweenPlayer < 10f && timer >= SpawnProjectilesCooldown)
                {
                    anim.Play("glow");
                    SpawnProjectiles();
                    timer = 0;
                }
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
            //just in case
            if (isEnlarge)
            {
                attacks = giantBossAttacks;
            }
            else
            {
                attacks = miniBossAttacks;
            }
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
            if (isImmune || isEnlarge)
            {
                return;
            }

            float damage = getHitBy.damage * (1 - armour);
            currentHp -= damage;
            sr.BlinkWhite();
            if (!isEnlarge)
            {

                if (currentHp <= 0)
                {
                    attacks = giantBossAttacks; //set attacks of boss to giant boss
                    isImmune = true;
                    goingEnlarge = true;

                    anim.SetTrigger("Immune");
                    // rageMode = true;
                    isEnlarge = true; // set to isEnlarge boss mode
                }
            }
        }

        public void GetHitFromWeakness(AttackArguments getHitBy)
        {
            if (isImmune)
            {
                return;
            }

            float damage = getHitBy.damage * (1 - armour);
            currentHp -= damage;
            sr.BlinkWhite();

            if (currentHp < (maxHp / 3) && !rageMode)
            {
                rageMode = true;
                SpawnProjectiles();
            }
            if (currentHp <= 0)
            {
                anim.Play("death");
                isdead = true;
                leftShoulder.gameObject.SetActive(false);
                rightShoulder.gameObject.SetActive(false);
                head.gameObject.SetActive(false);
                DropExperience();
            }

        }
        public void OnTriggerEnter2D(Collider2D col)
        {
            if (isEnlarge)
            {

            }
            else
            {
                if (col.CompareTag("Player") && currentAttack.trigger != "Laser")
                {
                    var attackArgs = GetAttackArgs(currentAttack);
                    Debug.Log("OnTrigger" + currentAttack.trigger);
                    col.gameObject.GetComponent<PlayerOnHit>().GetHit(attackArgs);
                }
            }



        }

        public void StopEnlarge()
        {

            //change the real scale 
            transform.localScale = new Vector3(4, 3, 1);

            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = true;

            isImmune = false;
            goingEnlarge = false;
            // isEnlarge = true;

            EnterGiantMode();
        }

        public void EnterGiantMode()
        {
            maxHp = 100;
            currentHp = 100;
            ShowWeaknessPoints();
        }

        public void ShowWeaknessPoints()
        {
            leftShoulder.Show(30);
            rightShoulder.Show(30);
            head.Show(40);

        }

        public void ShootArmProjectile()
        {
            Instantiate(ProjectilePrefab, LaunchArmProjectileOffset.position, transform.rotation);
        }

        public void SpawnProjectiles()
        {
            NumberOfSpawns = Random.Range(1, 4);

            Invoke("SpawnBomb", 1f);//call spawn bomb method with 1 sec  delay;

        }

        private void SpawnBomb()
        {
            //to spawn the bomb x number of spawns
            if (NumberOfSpawns <= 0)
            {
                return;
            }

            Instantiate(bombPrefab, ProjectileSpawnPoint1.position, transform.rotation);
            NumberOfSpawns--;
            Invoke("SpawnBomb", 1f);
        }

        public void OnSheildCast()
        {
            armour = 0.5f; //armour = 30%
            StartCoroutine(ResetSheild());
        }

        IEnumerator ResetSheild()
        {
            yield return new WaitForSeconds(5);
            armour = 0; //reset armour to zero
        }


        // public void EnterRageMode(){

        // }

        // private void OnDrawGizmosSelected()
        // {
        //     // if (isEnlarge)
        //     // {
        //     //     Gizmos.color = Color.cyan;
        //     //     Gizmos.DrawWireSphere(weakness_head.position, weaknessRadius);
        //     //     Gizmos.DrawWireSphere(weakness_leftshoulder.position, weaknessRadius);
        //     //     Gizmos.DrawWireSphere(weakness_rightshoulder.position, weaknessRadius);
        //     // }


        // }
    }

}
