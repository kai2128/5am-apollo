using UnityEngine;
namespace Class
{
    public class AttackArguments
    {
        public Transform transform;
        public Vector2 dir;
        public float damage;
        public float force = 0;
        public float facing; // -1 or 1 (-1 means left, 1 means right)
        
        public AttackArguments(Transform transform, float damage, float force = 0)
        {
            facing =  transform.localScale.x < 0 ? -1 : 1;
            dir = facing < 0 ? Vector2.left : Vector2.right;
            this.transform = transform;
            UpdateDamage(damage, force);
        }
        public AttackArguments(float damage, float force = 0)
        {
            UpdateDamage(damage, force);
        }

        private void UpdateDamage(float dmg, float frc = 0)
        {
            damage = dmg;
            if (frc == 0)
                force = 0.2f * dmg; // calculate force base on damage   
            else
                force = frc;
        }

        public void Reset()
        {
            damage = 0;
            force = 0;
        }

        public AttackArguments UpdateTransform(Transform trans)
        {
            return SetTransform(trans).Refresh();
        }
        public AttackArguments SetTransform(Transform trans)
        {
            transform = trans;
            return this;
        }
        
        public AttackArguments Refresh(bool hasForce = true)
        {
            facing =  transform.localScale.x < 0 ? -1 : 1;
            dir = facing < 0 ? Vector2.left : Vector2.right;
            if(hasForce && force == 0)
                force = 0.2f * damage; // calculate force base on damage
            return this;
        }

        public AttackArguments()
        {
        }
    }
}