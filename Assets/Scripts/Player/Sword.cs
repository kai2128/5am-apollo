using System;
using Class;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class Sword : MonoBehaviour
    {
        private PlayerAnimation _anim;
        private PlayerCollision _col;
        private PlayerMovement _movement;

        public float interval = 2f;
        private float _timer;
        private string _attackType;

        public float[] forwardForces = { 0.3f, 0.2f, 0.5f };
        public float[] lightDamages = { 5, 3, 6 };
        public float airForwardForces = 0.2f;
        public float airDamage = 2f;
        public float groundForce = 3f;
        public float groundDamage = 8f;


        private AttackArguments _atkArgs = new();

        // Start is called before the first frame update
        void Start()
        {
            _anim = PlayerManager.Instance.playerAnim;
            _movement = PlayerManager.Instance.playerMovement;
            _col = PlayerManager.Instance.playerCol;
        }


        // Update is called once per frame
        void Update()
        {
            if (PlayerManager.Instance.isAttacking && PlayerManager.Instance.isDashing)
            {
                return;
            }

            LightAttack();
            AirAttack();
            RangedAttack();
        }

        void AirAttack()
        {
            if (!_col.onGround && Input.GetButtonDown("Fire1") && Input.GetAxis("Vertical") >= 0)
            {
                PlayerManager.Instance.isAttacking = true;
                PlayerManager.Instance.comboStep = 1;
                _atkArgs = new AttackArguments(airDamage);
                _anim.SetTrigger("swordAttackAir");
                _movement.PauseForAnimation();
                _movement.MoveForward(airForwardForces);
            }
            else if (!_col.onGround && Input.GetButtonDown("Fire1"))
            {
                PlayerManager.Instance.isAttacking = true;
                PlayerManager.Instance.comboStep = 1;
                _atkArgs = new AttackArguments(groundDamage, groundForce);
                _anim.SetTrigger("swordAttackGround");
                _movement.PauseForAnimation();
                _movement.MoveToGround(groundForce);
            }
        }

        void RangedAttack()
        {
            if (Input.GetButtonDown("Fire2") && _col.onGround)
            {
                PlayerManager.Instance.isAttacking = true;
                PlayerManager.Instance.comboStep = 1;
                _anim.SetTrigger("swordAttackRanged");
                _movement.rb.velocity = Vector2.zero;
                _movement.PauseForAnimation();
            }
        }

        void LightAttack()
        {
            if (_col.onGround && Input.GetButtonDown("Fire1"))
            {
                PlayerManager.Instance.isAttacking = true;
                PlayerManager.Instance.comboStep++;
                if (PlayerManager.Instance.comboStep > 3)
                    PlayerManager.Instance.comboStep = 1;
                _timer = interval;
                _atkArgs = new AttackArguments(lightDamages[PlayerManager.Instance.comboStep - 1]);
                _anim.SetTrigger("swordAttack1");
                _movement.PauseForAnimation();
                _movement.MoveForward(forwardForces[PlayerManager.Instance.comboStep - 1]);
            }

            // reset combo counter
            if (_timer != 0)
            {
                _timer -= Time.deltaTime;
                if (_timer < 0)
                {
                    _timer = 0;
                    PlayerManager.Instance.comboStep = 1;
                }
            }
        }

        public void OnHitEnemy(Collider2D col)
        {
            DOVirtual.Float(.2f, 1f, 0.6f, duration => _anim.anim.speed = duration);
            col.gameObject.GetComponent<Enemy.Enemy>()
                .GetHit(_atkArgs.UpdateTransform(PlayerManager.Instance.transform));
        }

        public void updateDamage(float multiplier)
        {
            for (int i = 0; i < lightDamages.Length; i++)
            {
                lightDamages[i] *= multiplier;
            }
            airDamage *= multiplier;
            groundDamage *= multiplier;
        }
    }
}