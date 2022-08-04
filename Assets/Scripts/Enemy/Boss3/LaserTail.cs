using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTail : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // var attackArgs = GetAttackArgs(currentAttack);
            // col.gameObject.GetComponent<PlayerOnHit>().GetHit(attackArgs);
            Debug.Log("Hit Player");
        }
    }
}
