using System.Collections;
using System.Collections.Generic;
using Player;
using Enemy;
using Class;
using UnityEngine;


public class SpellAnimation : MonoBehaviour
{
    public AttackArguments atkArgs = new AttackArguments(20f, 5f);

    void Start()
    {
    }

    void Update()
    {
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            col.gameObject.GetComponent<PlayerOnHit>().GetHit(atkArgs.UpdateTransform(transform));
    }

}


