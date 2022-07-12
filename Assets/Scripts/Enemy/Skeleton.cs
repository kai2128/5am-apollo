using System;
using Class;
using UnityEngine;
using Utils;

namespace Enemy
{
    public class Skeleton : Enemy
    {
        public float getHitForce;
        private Vector2 _dir;
        
        public override void GetHit(AttackArguments atkArgs)
        {
            transform.LookAtTarget(atkArgs.transform);
            isHit = true;
            getHitForce = atkArgs.force;
            _dir = transform.GetOppositeDirection();
            anim.SetTrigger("hit");
        }

        private void Update()
        {
            animInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (isHit)
            {
                rb.velocity = _dir * getHitForce;
                if (animInfo.normalizedTime >= .6f)
                    isHit = false;
            }
        }
    }
}