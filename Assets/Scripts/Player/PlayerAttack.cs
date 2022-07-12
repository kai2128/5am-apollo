using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public GameObject[] skills;
        public int currentSkill = 0;

        // Start is called before the first frame update
        void Start()
        {
            skills.ToList().ForEach(skill => skill.SetActive(false));
            skills[currentSkill].SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            SwitchSkill();
        }

        void SwitchSkill()
        {
            if (!Input.GetKeyDown(KeyCode.Q) && !Input.GetKeyDown(KeyCode.E)) return;
            
            skills[currentSkill].SetActive(false);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (--currentSkill < 0)
                    currentSkill = skills.Length - 1;                    
            }else if (Input.GetKeyDown(KeyCode.E))
            {
                if (++currentSkill > skills.Length - 1)
                    currentSkill = 0;
            }
            skills[currentSkill].SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Sword sd = GetComponentInChildren<Sword>();
            
            if(currentSkill == 0)
                DOVirtual.Float(.2f, 1f, 0.7f, duration => GetComponentInChildren<Animator>().speed = duration);
            
            if (col.CompareTag("Enemy") && currentSkill == 1)
                sd.OnHitEnemy(col);
        }
    }
}