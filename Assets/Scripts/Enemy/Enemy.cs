﻿using System;
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
        protected Vector3 startingPosition;

        public float currentHp;
        public float maxHp;

        protected void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            startingPosition = transform.position;
        }

        public abstract void GetHit(AttackArguments atkArgs);

        protected void FlipDirection()
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}