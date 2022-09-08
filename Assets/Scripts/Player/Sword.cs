using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Class;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using Debug = UnityEngine.Debug;

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

        [Header("Ranged attack")]
        public GameObject rangedEffect;
        public float rangedDamage = 4f;
        public float rangedForce = 1f;
        public float rangedSpeed = 1f;
        public float maximumTravelDistance = 5f;
        [SerializeField] private List<GameObject> rangedEffectPool = new();


        private AttackArguments _atkArgs = new();
        [Header("Sound Effect")]
        [SerializeField] private AudioSource swordSoundEffect;
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
                swordSoundEffect.Play();
                PlayerManager.Instance.isAttacking = true;
                PlayerManager.Instance.comboStep = 1;
                _atkArgs = new AttackArguments(airDamage);
                _anim.SetTrigger("swordAttackAir");
                _movement.PauseForAnimation();
                _movement.MoveForward(airForwardForces);
            }
            else if (!_col.onGround && Input.GetButtonDown("Fire1"))
            {
                swordSoundEffect.Play();
                PlayerManager.Instance.isAttacking = true;
                PlayerManager.Instance.comboStep = 1;
                _atkArgs = new AttackArguments(groundDamage, groundForce * .5f);
                _anim.SetTrigger("swordAttackGround");
                _movement.PauseForAnimation();
                _movement.MoveToGround(groundForce);
            }
        }

        void RangedAttack()
        {
            if (Input.GetButtonDown("Fire2") && _col.onGround)
            {
                swordSoundEffect.Play();
                PlayerManager.Instance.isAttacking = true;
                PlayerManager.Instance.comboStep = 1;
                _atkArgs = new AttackArguments(rangedDamage, rangedForce);
                _anim.SetTrigger("swordAttackRanged");
                _movement.rb.velocity = Vector2.zero;
                _movement.PauseForAnimation();
            }
        }

        public void CreateRangedEffect()
        {
            var availableRangedEffect = rangedEffectPool.FirstOrDefault(effect => effect.activeSelf == false);
            if (!availableRangedEffect)
            {
                availableRangedEffect = Instantiate(rangedEffect,
                    rangedEffect.GetComponent<SwordRangedEffect>().startPos.position, Quaternion.identity);
                rangedEffectPool.Add(availableRangedEffect);
            }
            availableRangedEffect.SetActive(true);
        }

        void LightAttack()
        {
            if (_col.onGround && Input.GetButtonDown("Fire1"))
            {
                swordSoundEffect.Play();
                PlayerManager.Instance.isAttacking = true;
                PlayerManager.Instance.comboStep++;
                if (PlayerManager.Instance.comboStep > 3)
                    PlayerManager.Instance.comboStep = 1;
                _timer = interval;
                _atkArgs = new AttackArguments(lightDamages[PlayerManager.Instance.comboStep - 1], 1f * PlayerManager.Instance.comboStep);
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
            DOVirtual.Float(.2f, 1f, 1.8f, duration => _anim.anim.speed = duration);
            col.gameObject.GetComponent<Enemy.Enemy>()
                .GetHit(_atkArgs.UpdateTransform(PlayerManager.Instance.transform));
        }

        public void DestroyTree(GameObject tree)
        {
            var tilemapRenderer = tree.GetComponentInChildren<TilemapRenderer>();
            tilemapRenderer.BlinkColor(new Color(255f, 255f, 255f), 1f);
            DOVirtual.DelayedCall(0.5f, () => Destroy(tree.gameObject));
        }

        public void UpdateDamage(float multiplier)
        {
            for (int i = 0; i < lightDamages.Length; i++)
            {
                lightDamages[i] *= multiplier;
            }
            airDamage *= multiplier;
            groundDamage *= multiplier;
            // airDamage += multiplier;
        }

        public void resetDamage(float multiplier)
        {
            for (int i = 0; i < lightDamages.Length; i++)
            {
                lightDamages[i] /= multiplier;
            }
            airDamage /= multiplier;
            groundDamage /= multiplier;
            // airDamage += multiplier;
        }
    }
}