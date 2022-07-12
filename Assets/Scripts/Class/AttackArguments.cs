using UnityEngine;
namespace Class
{
    public class AttackArguments
    {
        public Transform transform;
        public Vector2 dir;
        public float damage;
        public float force;
        public float facing; // -1 or 1 (-1 means left, 1 means right)
        
        public AttackArguments(Transform transform, float damage, float force = 0)
        {
            dir = transform.localScale;
            facing = dir.x > 0 ? -1 : 1;
            this.transform = transform;
            this.damage = damage;
            this.force = force;
        }
    }
}