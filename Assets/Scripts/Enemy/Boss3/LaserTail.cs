using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Enemy.Boss3
{
    public class LaserTail : MonoBehaviour
    {
        public Boss3 boss3;
        // Start is called before the first frame update
        void Start()
        {
            boss3 = GameObject.Find("Boss_3").GetComponent<Boss3>();
        }

        // Update is called once per frame
        void Update()
        {
            boss3.currentAttack = boss3.laser;
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                var attackArgs = boss3.laser.GetAttackArgs().UpdateTransform(boss3.transform);
                col.gameObject.GetComponent<PlayerOnHit>().GetHit(attackArgs);
                Debug.Log("Hit Player");
            }
        }
    }
}

