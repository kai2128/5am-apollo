using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Class;
using static Utils.Utils;
using Utils;
namespace Enemy.Boss3
{
    public class Boss3RightShoulder : Enemy
    {

        public Boss3 boss;
        // Start is called before the first frame update
        void Awake()
        {
            // currentHp = boss.maxHp / 3;

        }

        // Update is called once per frame
        void Update()
        {
            if (boss.isEnlarge)
            {
                if (boss.isShooting)
                {
                    sr.enabled = false;
                    anim.enabled = false;

                }
                else
                {

                    sr.enabled = true;
                    anim.enabled = true;
                }
            }

        }

        public void Show(float hp)
        {
            // currentHp = hp / 3; //update hp hold by this weakness point with new max hp of boss
            currentHp = hp;
            sr.enabled = true;
            anim.enabled = true;

        }


        public override void GetHit(AttackArguments getHitBy)
        {
            if (boss.isImmune)
            {
                return;
            }
            if (boss.isShooting)
            {
                return;
            }
            if (currentHp > 0)
            {
                boss.GetComponent<Animator>().ResetAllTriggers();
                boss.GetHitFromWeakness(getHitBy); //deduct boss damage
                float damage = getHitBy.damage * (1 - boss.armour);
                currentHp -= damage; //deduct hp hold for this weakness point

            }

            if (currentHp <= 0 && boss.isEnlarge)
            {
                Debug.Log("HP Emptied");
                sr.enabled = false;
                anim.enabled = false;
                gameObject.SetActive(false);
                boss.shoot.SetWeight(-1);
                //check if this two weakness point inactve then dont flip
                if (!boss.rightShoulder.gameObject.activeSelf && !boss.head.gameObject.activeSelf)
                {
                    boss.canFlip = false;
                }
            }



        }

    }

}
