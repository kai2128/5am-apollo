using Class;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class Claw : MonoBehaviour
    {
        public float clawDamage = 3.0f;
        public void EndAttack()
        {
            PlayerManager.Instance.isAttacking = false;
            gameObject.SetActive(false);
        }  

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                col.gameObject.GetComponent<Enemy.Enemy>().GetHit(new AttackArguments(PlayerManager.Instance.transform, clawDamage));
            }
        }

        public void updateDamage(float multiplier)
        {
            clawDamage *= multiplier;
        }
    }
}
