using System;
using Cinemachine;
using DG.Tweening;
using Player;
using UnityEngine;

namespace Enemy
{
    public class BossEntrance : MonoBehaviour
    {
        public GameObject boss;
        public GameObject revealEffect;
        public GameObject fillBossHealth;
        public PolygonCollider2D confiner;
        public CinemachineVirtualCamera vcam;

        private BoxCollider2D _boxCol;
        private SpriteRenderer _sr;
        
        // Start is called before the first frame update
        void Start()
        {
            _boxCol = GetComponent<BoxCollider2D>();
            _sr = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                var orthographicSize = vcam.m_Lens.OrthographicSize;
                var player = vcam.m_Follow;
                vcam.gameObject.SetActive(true);

                // enter boss fight
                revealEffect.GetComponent<Animator>().SetTrigger("reveal");
                
                DOVirtual.Float(orthographicSize, 7, 2, (float size) =>
                {
                    vcam.m_Lens.OrthographicSize = size;
                });
                DOVirtual.Color(_sr.color, new Color(8, 8, 8), 2f, (Color color) =>
                {
                    _sr.color = color;
                });
                PlayerManager.Instance.canMove = false;
                PlayerManager.Instance.rb.velocity = Vector2.left * 5;
                DOVirtual.DelayedCall(1f, () =>
                {
                    vcam.m_Follow = boss.transform;
                    PlayerManager.Instance.rb.velocity = Vector2.zero;
                    _boxCol.isTrigger = false;
                });
                DOVirtual.DelayedCall(2f, () =>
                {
                    boss.GetComponent<Animator>().enabled = true;
                    fillBossHealth.SetActive(true);
                });
                DOVirtual.DelayedCall(5f, () =>
                {
                    vcam.m_Follow = player;
                    PlayerManager.Instance.canMove = true;
                    vcam.m_Lens.OrthographicSize = orthographicSize;
                });
            }
        }

        // private void OnTriggerExit2D(Collider2D other)
        // {
        //     if (other.CompareTag("Player"))
        //     {
        //         PlayerManager.Instance.canMove = true;
        //     }
        // }
    }
}
