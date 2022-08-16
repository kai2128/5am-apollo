using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Class;

namespace Enemy.Boss3
{
    public class ProjectileBehavior : MonoBehaviour
    {

        public float speed = 4f;
        public GameObject launchArmPoint;
        public GameObject target;
        private float launchArmPointX;
        private float targetX;
        private float dist;
        private float nextX;
        private float baseY;
        private float height;
        private AttackArguments attack;
        public Boss3 boss3;

        // Start is called before the first frame update
        void Start()
        {
            launchArmPoint = GameObject.FindGameObjectWithTag("LaunchProjectilePoint");

            target = GameObject.FindGameObjectWithTag("Player");
            boss3 = GameObject.Find("Boss_3").GetComponent<Boss3>();
            attack = boss3.shoot.GetAttackArgs().UpdateTransform(boss3.transform);
            Debug.Log("attack" + attack);
        }

        // Update is called once per frame
        void Update()
        {
            launchArmPointX = launchArmPoint.transform.position.x;
            targetX = target.transform.position.x;
            dist = targetX - launchArmPointX;
            nextX = Mathf.MoveTowards(transform.position.x, target.transform.position.x, speed * Time.deltaTime);
            baseY = Mathf.Lerp(Mathf.Abs(launchArmPoint.transform.position.y), Mathf.Abs(target.transform.position.y), (nextX - launchArmPointX) / dist);

            // height = 2 * (nextX - launchArmPointX) * (nextX - targetX) / (-0.25f * dist * dist);
            Vector3 movePosition = new Vector3(nextX, -baseY, transform.position.z);
            transform.rotation = LookAtTarget(movePosition - transform.position);
            transform.position = movePosition;
            // transform.position += transform.right * Time.deltaTime * speed;
            Destroy(gameObject, 2f);
        }

        public static Quaternion LookAtTarget(Vector2 rotation)
        {
            // Debug.Log(Mathf.Atan2(rotation.y, rotation.x) + Mathf.Rad2Deg);
            return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.name.Equals("Player"))
            {
                Debug.Log("Hit");
                collision.gameObject.GetComponent<PlayerOnHit>().GetHit(attack);
                Destroy(gameObject);
            }
            // Destroy(gameObject);

        }
    }
}
