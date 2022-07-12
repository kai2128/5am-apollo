using System.Linq;
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
    }
}