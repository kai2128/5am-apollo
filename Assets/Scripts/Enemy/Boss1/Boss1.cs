using System;
using System.Collections;
using UnityEngine;
using Class;
using DG.Tweening;
using Player;
using Utils;

namespace Enemy.Boss1
{
    public class Boss1 : Enemy
    {
        [HideInInspector] public static Boss1 Inst { get; private set; }
        
        private bool isFlipped;
        private Transform player;
        public Transform spawnPoint;

        public new string name;

        [Header("Properties")]
        public float jumpForce = 30f;
        [Header("Attacks")]
        public float attack1Damage = 20f;
        public float attack1Force = 1f;
        public float attack2Damage = 25f;
        public float attack2Force = 2f;
        public float attack3Damage = 25f;
        public float attack3Force = 3f;
        public float attack4Damage = 25f;
        public float attack4Force = 15f;
        public float attack5Damage = 30f;
        public float attack5Force = 30f;
        [HideInInspector] public AttackArguments _attackArgs;  
        
        
        [Header("States")]
        public float recoverTime = 3f;
        public float tenacity = 100f;
        public bool isStunned = false;
        public bool rageMode = false;
        public bool dead = false;
        // Start is called before the first frame update
        private void Awake()
        {
            Inst = this;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            currentHp = maxHp;
            name = "Meta Knight";
        }

        public override void GetHit(AttackArguments atkArgs)
        {
            if(dead)
                return;
            
            if (atkArgs.dir.x != transform.GetFacingDirection().x)
                return;
            
            currentHp -= atkArgs.damage;
            StartCoroutine(BlinkWhite());
            ReduceTenacity(atkArgs);

            if (currentHp / maxHp < .4 && !rageMode)
            {
                rageMode = true;
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
            anim.SetFloat("verticalVelocity", rb.velocity.x);
            anim.SetFloat("horizontalVelocity", rb.velocity.y);
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (rageMode)
                    _attackArgs.damage *= 1.3f;
                col.gameObject.GetComponent<PlayerOnHit>().GetHit(_attackArgs);
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
