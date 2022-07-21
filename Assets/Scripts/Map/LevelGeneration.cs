using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public GameObject[] rooms; //0:LeftS, 1:RightS, 2:Hole, 3:Jump, 4:MP
    public GameObject[] minions; // to call minion that spawn on the map

    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.1f;

    private float minX;
    private float maxX;
    private float minY;
    private bool stopGeneration;

    public int level;

    public Transform pos1;
    public Transform pos2;

    // Start is called before the first frame update
    void Start()
    {
        minX = pos1.position.x;
        maxX = pos2.position.x;
        Debug.Log(minX);
        minY = pos1.position.y;
        if (level == 1)
        {
            transform.position = pos1.position;
            direction = 1;
            Instantiate(rooms[0], transform.position, Quaternion.identity);
        }
        else {

            transform.position = pos2.position;
            direction = 2;
            Instantiate(rooms[1], transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
       if (timeBtwRoom <= 0 && stopGeneration == false)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else {
            timeBtwRoom -= Time.deltaTime;
        }
    }

    private void Move() {

        if (direction == 1)
        {
            Debug.Log(transform.position);
            if (transform.position.x < maxX)
            {
                // Move right 
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;
                int randRoom = Random.Range(2, 5);
                Instantiate(rooms[randRoom], transform.position, Quaternion.identity);
                //spawn minion
                int randMinionCount = Random.Range(0, minions.Length);
                Vector2 minionPos = new Vector2(Random.Range(transform.position.x, transform.position.x + 5.0f), Random.Range(minY + 1.0f, minY + 5.0f));
                if (minions != null || minions.Length != 0)
                {
                    Instantiate(minions[randMinionCount], minionPos, Quaternion.identity);
                }
            }
            else {
                stopGeneration = true;
            }
        }
        else if(direction == 2) {

            Debug.Log(transform.position);
            if (transform.position.x > minX) 
            {
                //Move left
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;
                int randRoom = Random.Range(2, 5);
                Instantiate(rooms[randRoom], transform.position, Quaternion.identity);
                // spawn minion
                int randMinionCount = Random.Range(0, minions.Length);
                Vector2 minionPos = new Vector2(Random.Range(transform.position.x,transform.position.x + 5.0f), Random.Range(minY + 1.0f, minY + 5.0f)) ;
                if (minions != null || minions.Length != 0)
                {
                    Instantiate(minions[randMinionCount], minionPos, Quaternion.identity);
                }
            }
            else
            {
                stopGeneration = true;
            }
        }
    }
}
