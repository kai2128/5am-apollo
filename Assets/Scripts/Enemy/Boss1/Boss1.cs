using System;
using UnityEngine;
using Class;
using Player;

namespace Enemy.Boss1
{
    public class Boss1 : Enemy
    {
        private bool isFlipped;
        private Transform player;
        public Transform spawnPoint;

        public new string name;
        public float jumpForce = 30f;

        // Start is called before the first frame update
        private new void Start()
        {
            base.Start();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            currentHp = maxHp;
            name = "Meta Knight";
        }

        public override void GetHit(AttackArguments atkArgs){}

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerOnHit>().GetHit(new AttackArguments(transform,20, 5));
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
            anim.enabled = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPoint.position, 1);
        }
    }
}
