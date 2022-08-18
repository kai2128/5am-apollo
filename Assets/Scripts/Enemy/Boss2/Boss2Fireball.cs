using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Boss2
{
    public class Boss2Fireball : StateMachineBehaviour
    {
        private Transform player;
        private Rigidbody2D rb;
        private Boss2 boss;
        private float timer = 0;
        public Fireball fireball;
        private List<Fireball> fireballs;

        private Vector2 startingPoint;

        private Vector3 dir;//ori dir
        private float Speed;

        private Vector2 target;
        private Transform point;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            boss = animator.GetComponent<Boss2>();
            Boss2.Attack attack = boss.attack2;
            startingPoint = boss.transform.position;
            rb = animator.GetComponent<Rigidbody2D>();
            dir = fireball.transform.localScale;
            Speed = 10f;
            fireballs = new List<Fireball>();
            point = boss.patrolPoint[3];

            if (boss.transform.position.x > player.position.x)
            {

                // create fireballs
                for (int i = 0; i < 5; i++)
                {
                    Fireball fb = Instantiate(fireball, null);
                    Vector3 dir = Quaternion.Euler(0, i * 15, 0) * -boss.transform.right;
                    fb.transform.position = boss.transform.position + dir * 1.0f;
                    fb.transform.rotation = Quaternion.Euler(0, 0, i * 15);
                    fireballs.Add(fb);
                }
            }
            else if (boss.transform.position.x < player.position.x)
            {
                // create fireballs
                for (int i = 5; i < 10; i++)
                {
                    Fireball fb = Instantiate(fireball, null);
                    Vector3 dir = Quaternion.Euler(0, i * 15, 0) * boss.transform.right;
                    fb.transform.position = boss.transform.position + dir * 1.0f;
                    fb.transform.rotation = Quaternion.Euler(0, 0, i * 15);
                    fireballs.Add(fb);
                }
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            boss.LookAtPlayer();
            timer += Time.deltaTime;
            if (timer >= boss.attack1.attackTime || boss.allowChase == false || boss.readyAttack == false)
            {
                boss.chase = false;
                animator.SetBool("Fireball", false);
                animator.SetBool("Ready", true);
                timer = 0;

            }
            else
            {
                // move fireball
                if (boss.transform.localScale.x < 0)
                {
                    foreach (Fireball fb in fireballs)
                    {
                        if (fb.hit == true)
                        {
                            fireballs.Remove(fb);
                            Destroy(fb.gameObject);
                            break;
                        }
                        else
                        {
                            fb.transform.localScale = new Vector3(dir.x, dir.y, dir.z);
                            fb.transform.position += Speed * -fb.transform.right * Time.deltaTime;

                        }
                    }
                }
                else if (boss.transform.localScale.x > 0)
                {
                    foreach (Fireball fb in fireballs)
                    {
                        if (fb.hit == true)
                        {
                            fireballs.Remove(fb);
                            Destroy(fb.gameObject);

                            break;
                        }
                        else
                        {
                            fb.transform.localScale = new Vector3(-dir.x, dir.y, dir.z);
                            fb.transform.position += Speed * fb.transform.right * Time.deltaTime;
                        }

                    }
                }

            }
            //boss.Ready();
            target = new Vector2(player.position.x, point.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, 2.5f * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

    }
}
