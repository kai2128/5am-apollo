using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerManager: MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }
    
        [Header("states")]
        public bool isAttacking = false;

        public bool canMove = true;
        public bool isDashing = false;

        [HideInInspector] 
        public PlayerAnimation playerAnim;
        public Transform playerTrans;
        public PlayerCollision playerCol;
        public PlayerMovement playerMovement; 
    
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);

            Instance = this;
            playerAnim = GetComponent<PlayerAnimation>();
            playerTrans = GetComponent<Transform>();
            playerMovement = GetComponent<PlayerMovement>();
            playerCol = GetComponent<PlayerCollision>();
        }


        public IEnumerator ToggleAttack(float after)
        {
            yield return new WaitForSeconds(after);
            isAttacking = !isAttacking;
        }
        
        public IEnumerator ToggleMovement(float after)
        {
            yield return new WaitForSeconds(after);
            canMove = !canMove;
        }
    }
}