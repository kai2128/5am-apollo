using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPosition;
    public GameObject[] rooms; //0:LeftS, 1:RightS, 2:Hole, 3:Jump, 4:MP
    
    [SerializeField]
    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;
    
    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private bool stopGeneration;

    public int level;

    public Transform pos1;
    public Transform pos2;

    [SerializeField]
    private bool startGeneration = false;
    private Action OnFinishGeneration;



    void Start()
    {
        // can change the starting position according to the level position later
        // int randStartingPos = Random.Range(0, startingPosition.Length);
    }
    
    public void StartGeneration(Action finishCallBack = null)
    {
        stopGeneration = false;
        minX = pos1.position.x;
        maxX = pos2.position.x;
        if (level == 1)
        {
            transform.position = startingPosition[0].position;
            direction = 1;
            GenerateRoom(rooms[0]);
        }
        else {
        
            transform.position = startingPosition[1].position;
            direction = 2;
            GenerateRoom(rooms[1]);
        }
        startGeneration = true;
        OnFinishGeneration = finishCallBack;
        OnFinishGeneration += () => startGeneration = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!startGeneration)
            return;
        
        if (timeBtwRoom <= 0 && stopGeneration == false)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else {
            timeBtwRoom -= Time.deltaTime;
        }

        if (stopGeneration)
        {
            OnFinishGeneration.Invoke();
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
                GenerateRandomRoom(2, 5);
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
                GenerateRandomRoom(2, 5);
            }
            else
            {
                stopGeneration = true;
            }
        }
    }

    private void GenerateRandomRoom(int min, int max)
    {
        GenerateRoom(rooms[Random.Range(min, max)]);
    }

    private void GenerateRoom(GameObject room)
    {
        var go = Instantiate(room, transform.position, Quaternion.identity);
        GameManager.Instance.SetParentToGenerated(go);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}
