using System;
using System.Collections;
using Class;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerOnHit : MonoBehaviour, IHittable
    {
        private Animator anim;
        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private CapsuleCollider2D col;
        [SerializeField] private AudioSource hitSoundEffect;
        private void Start()
        {
            anim = PlayerManager.Instance.anim;
            rb = PlayerManager.Instance.rb;
            sr = PlayerManager.Instance.sr;
            col = PlayerManager.Instance.col;
        }

        public void GetHit(AttackArguments args)
        {
            if (PlayerManager.Instance.isDeath || PlayerManager.Instance.isInvulnerable)
                return;
            if (args.damage > 15)
            {
                anim.SetTrigger("hit");
                PlayerManager.Instance.isAttacking = false;
            }
            else
            {
                sr.BlinkRed();
            }

            StartCoroutine(PlayerManager.Instance.playerMovement.PauseMovement(.2f));
            rb.velocity += args.PushBackwardForce(transform);
            DecreaseHp(args.damage);
        }


        public void DecreaseHp(float damage)
        {
            hitSoundEffect.Play();
            ref var currentHp = ref PlayerManager.Instance.currentHp;
            if (currentHp <= 0) return;

            if (damage >= currentHp)
                currentHp = 0;
            else
                currentHp -= damage;

            if (currentHp <= 0)
            {
                rb.AddForce(Vector2.down);
                PlayerManager.Instance.playerAnim.SetTrigger("die");
                sr.BlinkRed();
                PlayerManager.Instance.isDeath = true;
                PlayerManager.Instance.canMove = false;
            }
        }

    }
}