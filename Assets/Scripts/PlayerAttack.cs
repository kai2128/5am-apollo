using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerAnimation playerAnim;
    private GameObject clawEffect;
    private Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = transform.parent.gameObject.GetComponent<PlayerAnimation>();
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        transform.eulerAngles = new Vector3(x, y, 0);

        if (Input.GetButtonDown("Fire1") && !player.isDasing)
        {
            playerAnim.SetTrigger("claw");
            ClawEffect();
        }
    }


    private void ClawEffect()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
