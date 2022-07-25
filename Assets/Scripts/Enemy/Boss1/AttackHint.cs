using System;
using UnityEngine;
using Utils;

namespace Enemy.Boss1
{
    public class AttackHint : MonoBehaviour
    {
        private Animator anim;
        private SpriteRenderer sr;

        public enum AttackType
        {
            Danger, Normal, Medium, Special
        }
    
        void Awake()
        {
            anim = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (anim.HasPlayedOver())
            {
                gameObject.SetActive(false);
            }
        }
        
        public void HintAttack(AttackType type = AttackType.Normal)
        {
            switch (type)
            {
                case AttackType.Danger:
                    sr.material.color = Color.red;
                    break;
                case AttackType.Normal:
                    sr.material.color = Color.white;
                    break;
                case AttackType.Medium:
                    sr.material.color = Color.yellow;
                    break;
                case AttackType.Special:
                    sr.material.color = Color.cyan;
                    break;
            }
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            anim.Play("AttackHint");
        }
    }
}
