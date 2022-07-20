using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPosition;
    public GameObject[] rooms; //0:LeftS, 1:RightS, 2:Hole, 3:Jump, 4:MP

    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;

    private float minX;
    private float maxX;
    private bool stopGeneration;

    public int level;

    public Transform pos1;
    public Transform pos2;

    // Start is called before the first frame update
    void Start()
    {
        // can change the starting position according to the level position later
        // int randStartingPos = Random.Range(0, startingPosition.Length);
        minX = pos1.position.x;
        maxX = pos2.position.x;
        if (level == 1)
        {
            transform.position = startingPosition[0].position;
            direction = 1;
            Instantiate(rooms[0], transform.position, Quaternion.identity);
        }
        else {

            transform.position = startingPosition[1].position;
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
            if (transform.position.x < maxX)
            //if (transform.position.x < pos2.position.x)
            {
                // Move right 
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;
                int rand = Random.Range(2, 5);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);
            }
            else {
                stopGeneration = true;
            }
        }
        else if(direction == 2) {
            if (transform.position.x > minX) 
            //if(transform.position.x > pos1.position.x)
            {
                Debug.Log(pos1.position.x);
                //Move left
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;
                int rand = Random.Range(2, 5);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);
            }
            else
            {
                stopGeneration = true;
            }
        }
    }

}
