using System;
using Class;
using UnityEngine;
using Utils;

namespace Enemy
{
    public class Skeleton : Enemy
    {
        public float speed;
        private Vector2 _dir;
        
        public override void GetHit(AttackArguments atkArgs)
        {
            // var localScale = transform.localScale;
            // localScale.x = Math.Abs(localScale.x) *  atkArgs.facing;
            // transform.localScale = localScale;
            transform.LookAtTarget(atkArgs.transform);
            isHit = true;
            _dir = atkArgs.transform.localScale;
            anim.SetTrigger("hit");
        }

        private void Update()
        {
            animInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (isHit)
            {
                rb.velocity = _dir * speed;
                if (animInfo.normalizedTime >= .6f)
                    isHit = false;
            }
        }
    }
}