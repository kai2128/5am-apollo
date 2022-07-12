using UnityEngine;

namespace Player
{
    public class PlayerCollision : MonoBehaviour
    {
        [Header("Layers")]
        public LayerMask groundLayer;

        public LayerMask enemyMask;

        [Header("Collisions")]
        public bool onGround;
        public Vector2 bottomOffset;
        public float collisionRadius = 0.25f;

        // Update is called once per frame
        void Update()
        {
            Vector2 pos = transform.position;
            onGround = Physics2D.OverlapCircle(pos + bottomOffset, collisionRadius, groundLayer) ||
                       Physics2D.OverlapCircle(pos + bottomOffset, collisionRadius, enemyMask);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
        
            Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        }
    }
}
