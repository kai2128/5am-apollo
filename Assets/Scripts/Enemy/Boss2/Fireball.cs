using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Class;
using DG.Tweening;
using Player;
using static Utils.Utils;
using Utils;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using Range = Class.Range;

namespace Enemy.Boss2
{
    public class Fireball : MonoBehaviour
    {

        public AttackArguments atkArgs = new AttackArguments(10f, 2f);
        public bool hit;
        private Animator animator;
        public float lifetime;
        public bool isDestroy;
        private CircleCollider2D fbCollider;
        void Start()
        {

            animator = GetComponent<Animator>();
            lifetime = 2f;
            hit = false;
            isDestroy = false;
            fbCollider = transform.GetComponent<CircleCollider2D>();
        }

        void Update()
        {

        }
        public void Destroy()
        {
            hit = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.CompareTag("Ground"))
            {

                animator.SetBool("Hit", true);
                hit = true;
            }
          ;
            if (collision.CompareTag("Player"))//hit player
            {

                hit = true;
                animator.SetBool("Hit", true);

                //need to add the attack damage from boss
                collision.gameObject.GetComponent<PlayerOnHit>().GetHit(atkArgs.UpdateTransform(transform));

            }

        }
    }
}


