using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Enemy;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject level1Generator;
    public BossEntrance boss1Entrance;

    [HideInInspector]
    public GameObject generatedGameObject;

    public void SetParentToGenerated(GameObject go)
    {
        if (generatedGameObject == null)
        {
            generatedGameObject = new GameObject();
            generatedGameObject.name = "Generated Object";
        }
        go.transform.SetParent(generatedGameObject.transform);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);

        Instance = this;
        GenerateLevel1();
    }

    public void GenerateLevel1()
    {
        var levelGeneration = level1Generator.GetComponent<LevelGeneration>();
        levelGeneration.StartGeneration(() =>
        {
            boss1Entrance.MoveBossRoomTo(levelGeneration.pos2.transform.position + Vector3.up * 14 + Vector3.left * 2.5f);
            boss1Entrance.ResetBossRoom();
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere( level1Generator.GetComponent<LevelGeneration>().pos2.position, 1);
    }

    [ContextMenu("test")]
    public void OnPlayerRespawn()
    {
        Destroy(generatedGameObject);
        GenerateLevel1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
