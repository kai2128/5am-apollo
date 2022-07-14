using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerManager: MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }

        [Header("States")] 
        public bool isInvulnerable = false;
        public bool isAttacking = false;
        [Space]
        public bool canMove = true;
        public bool isDashing = false;
        public bool isDeath = false;
        public int comboStep = 1;
        public Vector2 spawnPoint;

        [Header("Properties")] 
        public float maxHp = 20;
        public float currentHp;
        
        [HideInInspector] 
        public PlayerAnimation playerAnim;
        public Transform playerTrans;
        public Animator anim;
        public SpriteRenderer sr;
        public Rigidbody2D rb;
        public PlayerCollision playerCol;
        public PlayerMovement playerMovement;
        public CapsuleCollider2D col;

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
        }

        public void becomeInvulnerable()
        {
            isInvulnerable = true;
            DOVirtual.DelayedCall(0.35f, becomeVulnerable, false);
        }
        public void becomeVulnerable()
        {
            isInvulnerable = false;
        }

        public void Die()
        {
            anim.enabled = false;
            DOVirtual.DelayedCall(1f, Instance.Respawn);
        }
        public void Respawn()
        {
            isDeath = false;
            canMove = true;
            transform.position = spawnPoint;
            anim.enabled = true;
            sr.enabled = true;
            currentHp = maxHp;
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
    }
}