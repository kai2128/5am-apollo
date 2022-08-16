using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectileBehavior : MonoBehaviour
{
    public float speed = 8f;
    public GameObject launchArmPoint;
    public GameObject target;
    private float launchArmPointX;
    private float targetX;
    private float dist;
    private float nextX;
    private float baseY;
    private float height;

    // Start is called before the first frame update
    void Start()
    {
        launchArmPoint = GameObject.FindGameObjectWithTag("SpawnProjectilePoint");
        Debug.Log(launchArmPoint.transform.position);
        target = GameObject.FindGameObjectWithTag("Player");


    }

    // Update is called once per frame
    void Update()
    {
        launchArmPointX = launchArmPoint.transform.position.x;
        targetX = target.transform.position.x;
        dist = targetX - launchArmPointX;
        nextX = Mathf.MoveTowards(transform.position.x, target.transform.position.x, speed * Time.deltaTime);
        baseY = Mathf.Lerp(launchArmPoint.transform.position.y, target.transform.position.y, (nextX - launchArmPointX) / dist);

        // height = 2 * (nextX - launchArmPointX) * (nextX - targetX) / (-0.25f * dist * dist);
        Vector3 movePosition = new Vector3(nextX, baseY, transform.position.z);
        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;
        // transform.position += transform.right * Time.deltaTime * speed;
        Destroy(gameObject, 4f);
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
            Destroy(gameObject);
        }
        // Destroy(gameObject);

    }
}
