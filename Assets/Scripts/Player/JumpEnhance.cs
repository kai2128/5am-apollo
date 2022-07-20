using UnityEngine;

namespace Player
{
    public class JumpEnhance : MonoBehaviour
    {
        private Rigidbody2D rb;

        public float fallMultiplier = 3f;
        public float lowJumpMultiplier = 1.5f;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if(PlayerManager.Instance.isDeath)
                return;
            
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }
}