using UnityEngine;
using Class;

namespace Enemy.Boss1
{
    public class Boss1 : Enemy
    {
        private bool isFlipped;
        private Transform player;

        public new string name;

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
    }
}
