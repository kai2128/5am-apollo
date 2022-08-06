using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Class;
using static Utils.Utils;
using Utils;
namespace Enemy.Boss3
{
    public class Boss3LeftShoulder : Enemy
    {

        public Boss3 boss;
        // Start is called before the first frame update
        void Awake()
        {
            currentHp = boss.maxHp / 3;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateCurrentHp(float hp)
        {
            currentHp = hp / 3;
        }

        public override void GetHit(AttackArguments getHitBy)
        {
            if (boss.isImmune)
            {
                return;
            }

            float damage = getHitBy.damage;
            currentHp -= damage;
            boss.currentHp -= damage;
            SpriteRenderer bossSR = boss.gameObject.GetComponent<SpriteRenderer>();
            bossSR.BlinkWhite();

            if (currentHp <= 0)
            {
                Debug.Log("HP Emptied");
            }



        }

    }

}
