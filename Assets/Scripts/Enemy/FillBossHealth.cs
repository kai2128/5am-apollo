using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class FillBossHealth : MonoBehaviour
    {
        public Image fillImage;
        public GameObject boss;
        private float maxHp;
        private float currentHp;
        private Slider slider;

        void Awake()
        {
            slider = GetComponent<Slider>();
        }

        void Update()
        {
                // currentHp = boss.currentHp;
                // maxHp = boss.maxHp;
                currentHp = 70;
                maxHp = 100;
            
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

