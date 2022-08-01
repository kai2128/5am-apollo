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
        public GameObject[] weapons;
        public int currentWeapon = 0;

        // Start is called before the first frame update
        void Start()
        {
            DisableAllWeapon();
            ActiveCurrentWeapon();
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerManager.Instance.isDeath)
            {
                DisableAllWeapon();
                return;
            }
            SwitchWeapon();
        }

        public void ActiveCurrentWeapon()
        {
            weapons[currentWeapon].SetActive(true);
        }
        void DisableAllWeapon()
        {
            weapons.ToList().ForEach(skill => skill.SetActive(false));
        }

        void SwitchWeapon()
        {
            if (!Input.GetKeyDown(KeyCode.Q)) return;

            weapons[currentWeapon].SetActive(false);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (--currentWeapon < 0)
                    currentWeapon = weapons.Length - 1;
            }
            weapons[currentWeapon].SetActive(true);
        }

        public void CreateSwordEffect()
        {
            PlayerManager.Instance.playerSword.CreateRangedEffect();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (currentWeapon == 0)
                DOVirtual.Float(.2f, 1f, 0.7f, duration => GetComponentInChildren<Animator>().speed = duration);

            if (col != null && col.CompareTag("Enemy") && currentWeapon == 1)
                PlayerManager.Instance.playerSword.OnHitEnemy(col);
            
            if (col.CompareTag("BlockingTree")  && currentWeapon == 1)
            {
                PlayerManager.Instance.playerSword.DestroyTree(col.gameObject);
            }
        }

        public void UpdateDamages(float multiplier)
        {
            weapons[0].transform.Find("Claw").gameObject.GetComponent<Player.Claw>().updateDamage(multiplier);
            weapons[1].GetComponent<Player.Sword>().UpdateDamage(multiplier);
        }
    }
}