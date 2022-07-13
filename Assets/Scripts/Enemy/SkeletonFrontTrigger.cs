using System;
using UnityEngine;

namespace Enemy
{
    public class SkeletonFrontTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            gameObject.transform.parent.GetComponentInParent<Skeleton>().OnFrontCollisionEnter(col);
        }
    }
}
