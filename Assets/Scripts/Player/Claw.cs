using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    public void EndAttack()
    {
        PlayerManager.Instance.isAttacking = false;
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            Debug.Log(1);
            col.gameObject.GetComponent<Enemy>().GetHit(transform.localScale);
        }
    }
}
