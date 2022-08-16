using System;
using Class;
using UnityEngine;
using Player;

namespace Enemy
{
    public abstract class Enemy : MonoBehaviour, IHittable
    {
        protected SpriteRenderer sr;
        public Rigidbody2D rb;
        protected AnimatorStateInfo animInfo;
        protected Animator anim;
        protected Vector3 startingPosition;
        public float currentHp;
        public float maxHp;
        public string enemyName;

        public float enemyXp;
        public int enemyLevel;

        protected void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            startingPosition = transform.position;
        }

        protected void DropExperience()
        {
            PlayerManager.Instance.playerLevel.GainExperienceFlatRate(enemyXp);
        }

        protected void FlipDirection()
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        public abstract void GetHit(AttackArguments getHitBy);
    }
}