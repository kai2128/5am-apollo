using System;
using Class;
using DG.Tweening;
using Player;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Froggie : Enemy
    {
        public enum State
        {
            Idle,
            Jump
        }

        public State currentState;
        [SerializeField]
        private int idleTimes;
        public float jumpForce = 7f;
        public float forwardForce = 5f;
        public bool getHit = false;
        public AttackArguments atkArgs = new(10f, 5f);
        public bool isDead = false;
        
        // Start is called before the first frame update
        void Awake()
        {
            maxHp = 20;
            currentHp = maxHp;
            enemyXp = 30;
            currentState = State.Idle;
        }

        public override void GetHit(AttackArguments atkArgs)
        {
            if(isDead)
                return;
            
            getHit = true;

            currentHp -= atkArgs.damage;
            rb.AddForce(atkArgs.PushBackwardForce(transform));
            sr.BlinkWhite();

            if (currentHp <= 0)
            {
                isDead = true;
                sr.BlinkWhite(1f);
                anim.enabled = false;
                DropExperience();
                DOVirtual.DelayedCall(1f, () =>
                {
                    Destroy(gameObject);
                });
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(isDead)
                return;
            if (col.CompareTag("Player"))
                col.gameObject.GetComponent<PlayerOnHit>().GetHit(atkArgs.UpdateTransform(transform));
        }

        // Update is called once per frame
        void Update()
        {
            if(isDead)
                return;
            switch (currentState)
            {
                case State.Idle:
                    UpdateIdleState();
                    break;
                case State.Jump:
                    UpdateJumpState();
                    break;
            }
        }
        
        private void SwitchState(State state)
        {
            switch (currentState)
            {
                case State.Idle:
                    ExitIdleState();
                    break;
                case State.Jump:
                    ExitJumpState();
                    break;
            }

            switch (state)
            {
                case State.Idle:
                    EnterIdleState();
                    break;
                case State.Jump:
                    EnterJumpState();
                    break;
            }
            currentState = state;
        }
        
        private void UpdateIdleState()
        {
            if (anim.HasPlayedOver(idleTimes))
            {
                SwitchState(State.Jump);
            }
        }

        private void UpdateJumpState()
        {
            if (rb.velocity.y < 0.01 && rb.velocity.y > -0.01)
            {
                SwitchState(State.Idle);
            }
        }

        private void EnterJumpState()
        {
            rb.velocity = new Vector2(transform.GetFacingFloat() * forwardForce, jumpForce);
            anim.Play("Jump");
        }

        private void EnterIdleState()
        {
            anim.Play("Idle");
            rb.velocity = Vector2.zero;
            idleTimes = Random.Range(1, 4);
            if(getHit)
                idleTimes = Random.Range(0, 1);
            if (Utils.Utils.Chances(.4f))
            {
                FlipDirection();
            }
        }

        private void ExitJumpState()
        {
        }

        private void ExitIdleState()
        {
        }
    }
}