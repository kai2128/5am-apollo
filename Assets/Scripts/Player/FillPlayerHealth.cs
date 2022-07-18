using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class FillPlayerHealth : MonoBehaviour
    {
        public Image fillImage;
        private float maxHp;
        private float currentHp;
        private Slider slider;

        void Awake()
        {
            slider = GetComponent<Slider>();
        }

        void Update()
        {
            currentHp = PlayerManager.Instance.currentHp;
            maxHp = PlayerManager.Instance.maxHp;
            
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

