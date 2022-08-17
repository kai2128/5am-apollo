using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Boss2
{
    public class ChaseControl : MonoBehaviour
    {
        public Boss2 boss;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                boss.chase = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                boss.chase = false;
            }
        }
    }

}
