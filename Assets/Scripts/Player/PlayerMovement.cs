using System.Collections;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [HideInInspector] public Rigidbody2D rb;
        private PlayerAnimation anim;
        private PlayerCollision coll;

        public float speed = 5;
        [Range(1, 10)] public float jumpVelocity = 7f;
        public int jumpCount = 2;

        [Header("Dash")] public bool canDash = true;
        public float dashSpeed = 10;
        public float dashTime = 0.4f;

        [Header("Sound Effect")]
        [SerializeField] private AudioSource jumpSoundEffect;
        [SerializeField] private AudioSource dashSoundEffect;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<PlayerAnimation>();
            coll = GetComponent<PlayerCollision>();
        }

        public void MoveForward(float force)
        {
            rb.velocity = new Vector2(transform.localScale.x * force, rb.velocity.y);
        }

        public void MoveToGround(float force)
        {
            rb.velocity = new Vector2(rb.velocity.x, Vector2.down.y * force);
        }

        public IEnumerator PauseMovement(float time)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            PlayerManager.Instance.canMove = false;

            yield return new WaitForSeconds(time);
            PlayerManager.Instance.canMove = true;
            rb.gravityScale = 1;
        }

        public IEnumerator ZeroGravity(float time)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(time);
            rb.gravityScale = 1;
        }

        public void PauseForAnimation(float percentage = 1)
        {
            AnimatorStateInfo stateInfo = anim.anim.GetCurrentAnimatorStateInfo(0);
            AnimatorClipInfo[] animatorClipInfo = anim.anim.GetCurrentAnimatorClipInfo(0);
            float totalDuration = animatorClipInfo[0].clip.length * stateInfo.normalizedTime;
            float duration = totalDuration * percentage;
            StartCoroutine(PauseMovement(duration));
        }

        // Update is called once per frame
        void Update()
        {
            if (!PlayerManager.Instance.canMove || PlayerManager.Instance.isDeath)
                return;

            if (Input.GetButtonDown("Dash") && canDash)
                StartCoroutine(Dash());

            if (PlayerManager.Instance.isAttacking)
                return;

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            float xRaw = Input.GetAxisRaw("Horizontal");
            float yRaw = Input.GetAxisRaw("Vertical");
            Vector2 destination = new Vector2(x, y);
            anim.FlipDirection(x);


            Walk(destination);
            anim.SetHorizontalMovement(x, y, rb.velocity.y);

            if (coll.onGround) jumpCount = 2;
            if (Input.GetButtonDown("Jump") && jumpCount > 1)
            {
                jumpSoundEffect.Play();
                anim.SetTrigger("jump");
                rb.velocity = Vector2.up * jumpVelocity;
                jumpCount--;
            }
        }

        public void Walk(Vector2 des)
        {
            if (PlayerManager.Instance.isDashing)
                if (des.x == 0)
                    return;

            rb.velocity = new Vector2(des.x * speed, rb.velocity.y);
        }

        public void UpdateSpeed(float multiplier)
        {
            speed *= multiplier;
            jumpVelocity *= multiplier;
            dashSpeed *= multiplier;
        }

        public void ResetSpeed(float multiplier)
        {
            speed /= multiplier;
            jumpVelocity /= multiplier;
            dashSpeed /= multiplier;
        }

        private IEnumerator Dash()
        {
            dashSoundEffect.Play();
            anim.SetTrigger("dash");
            canDash = false;
            PlayerManager.Instance.isAttacking = false;
            PlayerManager.Instance.isDashing = true;
            PlayerManager.Instance.canMove = false;
            rb.velocity = Vector2.zero;
            float originalGravity = rb.gravityScale;
            yield return new WaitForSeconds(0.1f);
            canDash = true;

            rb.gravityScale = 0.0001f;
            rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);

            StartCoroutine(PlayerManager.Instance.ToggleMovement(dashTime - 0.15f, true));
            yield return new WaitForSeconds(dashTime);
            rb.gravityScale = originalGravity;
            PlayerManager.Instance.isDashing = false;
        }


    }
}