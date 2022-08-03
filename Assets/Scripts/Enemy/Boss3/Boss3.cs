using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Class;
namespace Enemy.Boss3
{
    public class Boss3 : Enemy
    {
        public Transform player;
        public bool isFlipped = false;
        public float distanceBetweenPlayer;
        // Start is called before the first frame update
        void Awake()
        {

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
        public override void GetHit(AttackArguments getHitBy)
        {

        }
    }

}
