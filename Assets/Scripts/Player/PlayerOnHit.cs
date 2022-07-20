using System;
using System.Collections;
using Class;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerOnHit : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private CapsuleCollider2D col;
        private void Start()
        {
            anim = PlayerManager.Instance.anim;
            rb = PlayerManager.Instance.rb;
            sr = PlayerManager.Instance.sr;
            col = PlayerManager.Instance.col;
        }

        public void GetHit(AttackArguments args)
        {
            if (PlayerManager.Instance.isDeath)
                return;
            if (args.damage > 15)
                anim.SetTrigger("hit");
            else
                StartCoroutine(BlinkRed());
            DecreaseHp(args.damage);
        }


        private IEnumerator BlinkRed()
        {
            Color defaultColor = sr.material.color;
            sr.material.color = new Color(255, 1, 1);
            yield return new WaitForSeconds(0.2f);
            sr.material.color = defaultColor;
        }

        public void DecreaseHp(float damage)
        {
            ref var currentHp = ref PlayerManager.Instance.currentHp;
            currentHp -= damage;
            if (currentHp <= 0)
            {
                rb.AddForce(Vector2.down);
                PlayerManager.Instance.playerAnim.SetTrigger("die");
                StartCoroutine(BlinkRed());
                PlayerManager.Instance.isDeath = true;
                PlayerManager.Instance.canMove = false;
            }
        }

    }
}