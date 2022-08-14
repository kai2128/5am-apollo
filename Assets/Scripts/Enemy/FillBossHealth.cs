using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Enemy
{
    public class FillBossHealth : MonoBehaviour
    {
        public Image fillImage;
        public TextMeshProUGUI bossText;
        public GameObject boss;
        private float maxHp;
        private float currentHp;
        private Slider slider;

        void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            bossText.SetText(boss.GetComponent<Enemy>().enemyName);
            maxHp = boss.GetComponent<Enemy>().maxHp;
        }

        void Update()
        {
                // currentHp = boss.currentHp;
                // maxHp = boss.maxHp;
                currentHp = boss.GetComponent<Enemy>().currentHp;

            
                if(slider.value <= slider.minValue)
                {
                    fillImage.enabled = false;
                }
                
                if(slider.value > slider.minValue && !fillImage.enabled)
                {
                    fillImage.enabled = true;
                }
                
                float fillValue = currentHp / maxHp;
                
                if(fillValue <= slider.maxValue / 3)
                {
                    fillImage.color = Color.yellow;
                }
                else
                {
                    fillImage.color = Color.red;
                }

                slider.value = fillValue;
            
        }
    }
}

