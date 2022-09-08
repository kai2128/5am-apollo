using System;
using Class;
using DG.Tweening;
using Player;

using UnityEngine;
using Utils;
using Random = UnityEngine.Random;


namespace Enemy
{
    public class Boss4_Effect : Enemy
    {
        public SpellAnimation spl;

        public GameObject boss4;
        public Transform[] effectPos; //randomly choose one of these positions to spawn the effect
        // public Transform effectPos2;
        public int count; //how many effects to spawn
        private int chosenPos; //the position that was chosen

        public float pos2position = 5;

        float time;
        float timeDelay;

        void Awake()
        {
            boss4 = GameObject.Find("Boss_4");
        }

        new void Start()
        {
            time = 0f;
            timeDelay = 1f;
        }

        void Update()
        {
            time += Time.deltaTime;

            if (time >= timeDelay)
            {
                time = 0f;

                if (boss4.GetComponent<Boss_4>().attack2)
                {
                    if(effectPos.Length != 0)
                    {
                        count = Random.Range(1, 3);
                        for (int i = 0; i < count; i++)
                        {
                            chosenPos = Random.Range(0, effectPos.Length);
                            SpellAnimation spell = Instantiate(spl, effectPos[chosenPos].position, effectPos[chosenPos].rotation);
                        }
                    }
                    else
                    {
                        SpellAnimation spell = Instantiate(spl, PlayerManager.Instance.rb.position + Vector2.up * pos2position, Quaternion.identity);
                    }

                }
            }
        }

         public override void GetHit(AttackArguments getHitBy)
         {
         }
    }

}
