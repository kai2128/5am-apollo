using System;
using Class;
using UnityEngine;

namespace Enemy
{
    public abstract class Enemy : MonoBehaviour
    {
        protected SpriteRenderer sr;
        protected Rigidbody2D rb;
        protected AnimatorStateInfo animInfo;
        protected Animator anim;

        public bool isHit;

        protected void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        public abstract void GetHit(AttackArguments atkArgs);
    }
}