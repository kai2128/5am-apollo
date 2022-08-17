using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }

        [Header("States")] public bool isInvulnerable = false;
        public bool isAttacking = false;
        [Space] public bool canMove = true;
        public bool isDashing = false;
        public bool isDeath = false;
        public int comboStep = 1;
        public Vector2 spawnPoint;

        [Header("Properties")] public float maxHp = 20;
        public float currentHp;

        [HideInInspector] public PlayerAnimation playerAnim;
        [HideInInspector] public Transform playerTrans;
        [HideInInspector] public Animator anim;
        [HideInInspector] public SpriteRenderer sr;
        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public PlayerCollision playerCol;
        [HideInInspector] public PlayerMovement playerMovement;
        [HideInInspector] public CapsuleCollider2D col;
        [HideInInspector] public PlayerAttack playerAttack;
        [HideInInspector] public CinemachineVirtualCamera mainCamera;
        [HideInInspector] public Sword playerSword;
        public LevelSystem playerLevel;
        public float attackLevelMultiplier = 1.0f;
        public TextMeshProUGUI statusText;

        public void SetStatusMessage(string msg, float duration = 4f)
        {
            statusText.text = msg;
            statusText.DOFade(1, 0.5f).SetEase(Ease.OutQuint);
            statusText.DOFade(0, duration).SetEase(Ease.InQuint);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPoint, 1);
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);

            Instance = this;
            currentHp = maxHp;
            playerAnim = GetComponent<PlayerAnimation>();
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            col = GetComponent<CapsuleCollider2D>();
            anim = GetComponent<Animator>();
            playerTrans = GetComponent<Transform>();
            playerMovement = GetComponent<PlayerMovement>();
            playerCol = GetComponent<PlayerCollision>();
            playerAttack = GetComponent<PlayerAttack>();
            playerLevel = GetComponent<LevelSystem>();
            playerSword = GetComponentInChildren<Sword>();
            mainCamera = GameObject.FindGameObjectWithTag("MainCM").GetComponent<CinemachineVirtualCamera>();
        }

        public void BecomeInvulnerable()
        {
            isInvulnerable = true;
            DOVirtual.DelayedCall(0.35f, BecomeVulnerable, false);
        }

        public void BecomeVulnerable()
        {
            isInvulnerable = false;
        }

        public void Die()
        {
            anim.enabled = false;
            canMove = false;
            DOVirtual.DelayedCall(1f, Instance.Respawn);
        }

        public void Respawn()
        {
            GameManager.Instance.OnPlayerRespawn();
            isDeath = false;
            canMove = true;
            transform.position = spawnPoint;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 1;
            anim.enabled = true;
            sr.enabled = true;
            currentHp = maxHp;
            playerAttack.ActiveCurrentWeapon();
            mainCamera.MoveToTopOfPrioritySubqueue();
        }


        public IEnumerator ToggleAttack(float after)
        {
            yield return new WaitForSeconds(after);
            isAttacking = !isAttacking;
        }

        public IEnumerator ToggleMovement(float after, bool? flag)
        {
            if (flag == null) flag = !canMove;
            yield return new WaitForSeconds(after);
            canMove = (bool)flag;
        }

        public void AttackOver()
        {
            isAttacking = false;
        }

        public void IncreaseHealth(int level)
        {
            var increment = (maxHp * 0.01f) * ((100 - level) * 0.1f);
            maxHp += increment;
            currentHp += increment;
        }

        public void IncreaseDmg(int level)
        {
            attackLevelMultiplier = 1 + ((100 - (100 - level)) * 0.1f);
            playerAttack.UpdateDamages(attackLevelMultiplier);
        }

        public void GrowDmg(bool growed)
        {
            if (growed)
            {
                playerAttack.UpdateDamages(1.3f);//increase ori damage by 30%
            }
            else
            {
                playerAttack.ResetDamages(1.3f);
            }

        }

        public void ShrinkDmg(bool shrinked)
        {
            if (shrinked)
            {
                playerAttack.UpdateDamages(0.7f);//decrease ori damage by 30%
            }
            else
            {
                playerAttack.ResetDamages(0.7f);
            }

        }

        public void GrowMovementSpeed(bool growed)
        {
            if (growed)
            {
                playerMovement.UpdateSpeed(0.5f);//decrease speed by 30%
            }
            else
            {
                playerMovement.ResetSpeed(0.5f);
            }
        }

        public void ShrinkMovementSpeed(bool shrinked)
        {
            if (shrinked)
            {
                playerMovement.UpdateSpeed(1.5f);//increase speed by 30%
            }
            else
            {
                playerMovement.ResetSpeed(1.5f);
            }
        }
    }
}