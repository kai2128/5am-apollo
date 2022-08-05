using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Boss3
{
    public class Laser : MonoBehaviour
    {
        Collider2D col;
        // public LineRenderer laserTail;
        // public Transform player;
        public Transform boss;
        public GameObject lineTail;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(false);
            lineTail.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            // if (gameObject.activeSelf)
            // {
            //     LaserEffect();
            // }
        }

        public void LaserEffect()
        {
            lineTail.SetActive(true);
        }
        public void EndLaserEffect()
        {
            gameObject.SetActive(false);
            lineTail.SetActive(false);
        }

    }

}