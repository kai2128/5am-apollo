using Class;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Player
{
    public class Claw : MonoBehaviour
    {
        public float originalClawDamage = 3f;
        public AttackArguments clawArgs = new AttackArguments(3f, 2f);
        public GameObject previousHit;
        public int damageCounter = 8;
        public int maxCounter = 8;
        public void EndAttack()
        {
            PlayerManager.Instance.isAttacking = false;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("BlockingTree"))
            {
                var tilemapRenderer = col.gameObject.GetComponentInChildren<TilemapRenderer>();
                tilemapRenderer.BlinkColor(new Color(1.5f, 1.5f, 1.5f));
            }

            if (col.CompareTag("Enemy"))
            {
                if (previousHit == null)
                {
                    previousHit = col.gameObject;
                    clawArgs.damage = originalClawDamage;
                }
                else if (previousHit == col.gameObject)
                {
                    if (damageCounter <= maxCounter) damageCounter++;
                    clawArgs.damage = originalClawDamage * Mathf.Pow(1.25f, damageCounter / 1.8f);
                }
                else
                {
                    previousHit = null;
                    damageCounter = 0;
                }

                col.gameObject.GetComponent<Enemy.Enemy>()
                    .GetHit(clawArgs.UpdateTransform(PlayerManager.Instance.transform));
            }
        }

        public void updateDamage(float multiplier)
        {
            originalClawDamage *= multiplier;
        }

        public void resetDamage(float multiplier)
        {
            originalClawDamage /= multiplier;
        }
    }
}