using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerAnimation playerAnim;
    private PlayerMovement playerMovement;
    private GameObject clawEffect;
    public float clawForwardForce = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = transform.parent.gameObject.GetComponent<PlayerAnimation>();
        playerMovement = transform.parent.gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        transform.eulerAngles = new Vector3(x, y, 0);

        if (Input.GetButtonDown("Fire1") && !PlayerManager.Instance.isDashing)
        {
            PlayerManager.Instance.isAttacking = true;
            playerAnim.SetTrigger("claw");
            StartCoroutine(playerMovement.PauseMovement(playerAnim.GetCurrentStateTime() + 0.5f));
            playerMovement.MoveForward(clawForwardForce);
            ClawEffect();
        }
    }


    private void ClawEffect()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

//     private void OnTriggerEnter2D(Collider2D col)
//     {
//         Debug.Log(col.tag);
//         if (col.CompareTag("Enemy"))
//         {
//             col.gameObject.GetComponent<Enemy>().GetHit(transform.localScale);
//         }
//     }
// }
}