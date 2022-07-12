using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        public Animator anim;

        private PlayerCollision col;
        [HideInInspector]
        public SpriteRenderer sr;
    
        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            col = GetComponentInParent<PlayerCollision>();
            sr = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            anim.SetBool("onGround", col.onGround);
            anim.SetBool("isAttacking", PlayerManager.Instance.isAttacking);
            anim.SetBool("isDashing", PlayerManager.Instance.isDashing);
            
            anim.SetInteger("comboStep", PlayerManager.Instance.comboStep);
        }

        public void SetHorizontalMovement(float x, float y, float yVel)
        {
            anim.SetFloat("horizontalAxis", Mathf.Abs(x));
            anim.SetFloat("verticalAxis", Mathf.Abs(y));
            anim.SetFloat("verticalVelocity", yVel);
        }

        public void SetTrigger(string trigger)
        {
            anim.SetTrigger(trigger);
        }

        public void FlipDirection(float side)
        {
            var facing = side switch
            {
                > 0 => 1,
                < 0 => -1,
                _ => transform.localScale.x 
            };
            transform.localScale = new Vector3(facing, 1, 1) ;
        }

        public float GetCurrentStateTime(int layer = 0)
        {
            var stateInfo = anim.GetCurrentAnimatorStateInfo(layer);
            float currentTime = stateInfo.normalizedTime % 1;
            return currentTime;
        }
    }
}
