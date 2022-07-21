using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    //public Transform[] startingPosition;
    public GameObject[] rooms; //0:LeftS, 1:RightS, 2:Hole, 3:Jump, 4:MP
    public GameObject[] minions; // to call minion that spawn on the map

    [SerializeField]
    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.1f;

    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float minY;
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
        minY = pos1.position.y;
        if (level == 1)
        {
            transform.position = pos1.position;
            direction = 1;
            GenerateRoom(rooms[0]);
        }
        else
        {
            transform.position = pos2.position;
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
        if (!startGeneration)
            return;

        if (timeBtwRoom <= 0 && stopGeneration == false)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime;
        }

        if (stopGeneration)
        {
            OnFinishGeneration.Invoke();
        }
    }

    private void Move()
    {

        if (direction == 1)
        {
            if (transform.position.x < maxX)
            {
                // Move right 
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;
                GenerateRandomRoom(2, 5);
                //spawn minion
                int randMinionCount = Random.Range(0, minions.Length);
                Vector2 minionPos = new Vector2(Random.Range(transform.position.x, transform.position.x + 5.0f), Random.Range(minY + 1.0f, minY + 5.0f));
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
        else if (direction == 2)
        {
            if (transform.position.x > minX)
            {
                Debug.Log(pos1.position.x);
                //Move left
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;
                GenerateRandomRoom(2, 5);
                //spawn minion
                int randMinionCount = Random.Range(0, minions.Length);
                Vector2 minionPos = new Vector2(Random.Range(transform.position.x, transform.position.x + 5.0f), Random.Range(minY + 1.0f, minY + 5.0f));
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