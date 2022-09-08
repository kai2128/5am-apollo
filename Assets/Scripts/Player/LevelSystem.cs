using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Player
{
    public class LevelSystem : MonoBehaviour
    {
        public int level;
        // public float totalXP;
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

        public TextMeshProUGUI XPText;
        public TextMeshProUGUI LevelText;
        // Start is called before the first frame update
        [Header("Sound Effect")]
        [SerializeField] public AudioSource levelSoundEffect;

        void Start()
        {
            frontXpBar.fillAmount = 0f;
            backXpBar.fillAmount = 0f;
            requiredXP = CalculateRequiredXp();
            LevelText.text = "Level " + level;
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
            XPText.text = Math.Round(currentXP) + "/" + Math.Round(requiredXP);
        }

        public void GainExperienceFlatRate(float xpGained)
        {
            currentXP += xpGained;
            lerpTimer = 0f;
            delayTimer = 0f;
        }

        public void LevelUp()
        {
            levelSoundEffect.Play();
            level++;
            // FillPlayerXP.Instance.levelUp();
            frontXpBar.fillAmount = 0f;
            backXpBar.fillAmount = 0f;
            currentXP = Mathf.RoundToInt(currentXP - requiredXP);
            PlayerManager playerManager = PlayerManager.Instance;
            playerManager.IncreaseHealth(level);
            playerManager.IncreaseDmg(level);
            requiredXP = CalculateRequiredXp();
            LevelText.text = "Level " + level;
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