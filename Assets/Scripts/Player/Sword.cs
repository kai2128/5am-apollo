using UnityEngine;

namespace Player
{
    public class Sword : MonoBehaviour
    {
        private PlayerAnimation _anim;
        private PlayerCollision _col;
        private PlayerMovement _movement;
        public float forwardForce1 = 0.3f;
        public float forwardForce2 = 0.2f;
        public float forwardForce3 = 0.5f;

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
            if (Input.GetButtonDown("Fire1") && !PlayerManager.Instance.isDashing && _col.onGround)
            {
                _anim.SetTrigger("swordAttack1");
                PlayerManager.Instance.isAttacking = true;
                StartCoroutine(PlayerManager.Instance.ToggleAttack(0.3f));
            }
        }
        
    }
}
