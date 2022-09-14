using UnityEngine;

namespace Player
{
    public class ClawAttack : MonoBehaviour
    {
        PlayerAnimation playerAnim;
        private PlayerMovement playerMovement;
        private GameObject clawEffect;
        public float clawForwardForce = 0.3f;
        [Header("Sound Effect")]
        [SerializeField] private AudioClip clawSoundEffect;

        // Start is called before the first frame update
        void Start()
        {
            playerAnim = PlayerManager.Instance.playerAnim;
            playerMovement = PlayerManager.Instance.playerMovement;
        }

        // Update is called once per frame
        void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            transform.eulerAngles = new Vector3(x, y, 0);

            if (Input.GetButtonDown("Fire1") && !PlayerManager.Instance.isDashing && !PlayerManager.Instance.isAttacking)
            {
                SoundManager.Instance.PlaySound(clawSoundEffect);
                PlayerManager.Instance.isAttacking = true;
                playerAnim.SetTrigger("claw");
                StartCoroutine(playerMovement.ZeroGravity(playerAnim.GetCurrentStateTime() + 0.5f));
                playerMovement.MoveForward(clawForwardForce);
                ClawEffect();
            }
        }


        private void ClawEffect()
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

    }
}