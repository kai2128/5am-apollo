using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityUI : MonoBehaviour
{
    [Header("Claw")]
    public Image claw;
    public KeyCode ability1; 

    [Header("Sword")]
    public Image sword;
    public KeyCode ability2; 

    [Header("Fly")]
    public Image fly;
    public float cooldown3 = 20;
    bool isCooldown3 = false;
    public KeyCode ability3; 

    [Header("Grow")]
    public Image grow;
    public float cooldown4 = 40;
    bool isCooldown4 = false;
    public KeyCode ability4; 

    void Update()
    {
        Weapon();
        Fly();
        Grow();
    }

    private void Weapon()
    {
        var currentActiveWeapon = PlayerManager.Instance.playerAttack.currentWeapon;
        if (currentActiveWeapon == 0)
        {
            claw.enabled = true;
            sword.enabled = false;
        }
        else
        {
            claw.enabled = false;
            sword.enabled = true;
        }
    }

    private void Fly()
    {
        if(!isCooldown3)
        {
            fly.fillAmount = 1;
        }

        if (Input.GetKey(ability3) && !isCooldown3)
        {
            isCooldown3 = true;
            fly.fillAmount = 1;
        }

        if (isCooldown3)
        {
            fly.fillAmount -= 1 / cooldown3 * Time.deltaTime;
            if (fly.fillAmount <= 0)
            {
                fly.fillAmount = 0;
                isCooldown3 = false;
            }
        }
    }

    private void Grow()
    {
        if(!isCooldown4)
        {
            grow.fillAmount = 1;
        }

        if (Input.GetKey(ability4) && !isCooldown4)
        {
            isCooldown4 = true;
            grow.fillAmount = 1;
        }

        if (isCooldown4)
        {
            grow.fillAmount -= 1 / cooldown4 * Time.deltaTime;
            if (grow.fillAmount <= 0)
            {
                grow.fillAmount = 0;
                isCooldown4 = false;
            }
        }
    }
}
