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
        
        public float[] forwardForces = {0.3f, 0.2f, 0.5f};

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
            Attack();
        }

        void Attack()
        {
            if (!PlayerManager.Instance.isAttacking && !PlayerManager.Instance.isDashing &&
                Input.GetButtonDown("Fire1"))
            {
                PlayerManager.Instance.isAttacking = true;
                PlayerManager.Instance.comboStep++;
                if (PlayerManager.Instance.comboStep > 3)
                    PlayerManager.Instance.comboStep = 1;
                _timer = interval;
                
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
        
    }
}
