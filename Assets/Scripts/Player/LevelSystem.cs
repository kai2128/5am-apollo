using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class LevelSystem : MonoBehaviour
    {
        public int level;
        public float currentXP;
        public float requiredXP;
        private float lerpTimer;
        private float delayTimer;
        [Header("UI")]
        public Image frontXpBar;
        public Image backXpBar;
        [Header("Multipliers")]
        [Range(1f, 300f)]
        public float additionMultiplier = 300;
        [Range(2f, 4f)]
        public float powerMultiplier = 2;
        [Range(7f, 14f)]
        public float divisionMultiplier = 7;
        // Start is called before the first frame update
        void Start()
        {
            frontXpBar.fillAmount = currentXP / requiredXP;
            backXpBar.fillAmount = currentXP / requiredXP;
            requiredXP = CalculateRequiredXp();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateXpUI();
            if (Input.GetKeyUp(KeyCode.P))
            {
                GainExperienceFlatRate(20);
            }
            if (currentXP > requiredXP)
            {
                LevelUp();
            }
        }

        public void UpdateXpUI()
        {
            float xpFraction = currentXP / requiredXP;
            float FPX = frontXpBar.fillAmount;
            if (FPX < xpFraction)
            {
                delayTimer += Time.deltaTime;
                backXpBar.fillAmount = xpFraction;
                if (delayTimer > 0.3)
                {
                    lerpTimer += Time.deltaTime;
                    float percentComplete = lerpTimer / 4;
                    frontXpBar.fillAmount = Mathf.Lerp(FPX, backXpBar.fillAmount, percentComplete);
                }
            }
        }

        public void GainExperienceFlatRate(float xpGained)
        {
            Debug.Log("Gain exp!");
            currentXP += xpGained;
            lerpTimer = 0f;
            delayTimer = 0f;
            return;
        }

        public void GainExperienceScalable(float xpGained, int passedLevel)
        {
            if (passedLevel < level)
            {
                float multipler = 1 + (level - passedLevel) * 0.1f;
                currentXP += xpGained * multipler;
            }
            else
            {
                currentXP += xpGained;
            }
            lerpTimer = 0f;
            delayTimer = 0f;
        }
        public void LevelUp()
        {
            level++;
            frontXpBar.fillAmount = 0f;
            backXpBar.fillAmount = 0f;
            currentXP = Mathf.RoundToInt(currentXP - requiredXP);
            Player.PlayerManager playerManager = PlayerManager.Instance;
            playerManager.IncreaseHealth(level);
            playerManager.IncreaseDmg(level);
            requiredXP = CalculateRequiredXp();
        }

        private int CalculateRequiredXp()
        {
            int solveForRequiredXp = 0;
            for (int levelCycle = 1; levelCycle <= level; levelCycle++)
            {
                solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
            }
            return solveForRequiredXp / 4;
        }
    }
}