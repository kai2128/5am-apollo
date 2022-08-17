using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Boss2
{
    public class ReadyControl : MonoBehaviour
    {
        public Boss2 boss;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                boss.readyAttack = true;
                boss.allowChase = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                boss.readyAttack = false;
                boss.allowChase = false;
            }
        }
    }

}
