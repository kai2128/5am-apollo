using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerManager: MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }

        [Header("states")] 
        public bool isInvulnerable = false;
        public bool isAttacking = false;
        [Space]
        public bool canMove = true;
        public bool isDashing = false;
        public int comboStep = 1;

        [Header("Properties")] 
        public float hp = 20;
        
        [HideInInspector] 
        public PlayerAnimation playerAnim;
        public Transform playerTrans;
        public Rigidbody2D rb;
        public PlayerCollision playerCol;
        public PlayerMovement playerMovement; 
    
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);

            Instance = this;
            playerAnim = GetComponent<PlayerAnimation>();
            rb = GetComponent<Rigidbody2D>();
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