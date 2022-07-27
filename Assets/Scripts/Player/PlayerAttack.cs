using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public GameObject[] skills;
        public int currentSkill = 0;

        // Start is called before the first frame update
        void Start()
        {
            DisableAllSKill();
            ActiveCurrentSkill();
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerManager.Instance.isDeath)
            {
                DisableAllSKill();
                return;
            }
            SwitchSkill();
        }

        public void ActiveCurrentSkill()
        {
            skills[currentSkill].SetActive(true);
        }
        void DisableAllSKill()
        {
            skills.ToList().ForEach(skill => skill.SetActive(false));
        }

        void SwitchSkill()
        {
            if (!Input.GetKeyDown(KeyCode.Q) && !Input.GetKeyDown(KeyCode.E)) return;

            skills[currentSkill].SetActive(false);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (--currentSkill < 0)
                    currentSkill = skills.Length - 1;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (++currentSkill > skills.Length - 1)
                    currentSkill = 0;
            }
            skills[currentSkill].SetActive(true);
        }

        public void CreateSwordEffect()
        {
            PlayerManager.Instance.playerSword.CreateRangedEffect();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (currentSkill == 0)
                DOVirtual.Float(.2f, 1f, 0.7f, duration => GetComponentInChildren<Animator>().speed = duration);

            if (col != null && col.CompareTag("Enemy") && currentSkill == 1)
                PlayerManager.Instance.playerSword.OnHitEnemy(col);
            
            if (col.CompareTag("BlockingTree")  && currentSkill == 1)
            {
                PlayerManager.Instance.playerSword.DestroyTree(col.gameObject);
            }
        }

        public void UpdateDamages(float multiplier)
        {
            skills[0].transform.Find("Claw").gameObject.GetComponent<Player.Claw>().updateDamage(multiplier);
            skills[1].GetComponent<Player.Sword>().UpdateDamage(multiplier);
        }
    }
}