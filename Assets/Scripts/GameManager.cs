using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
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
    public GameObject level2Generator;
    public Boss2Entrance boss2Entrance;

    public GameObject level3Generator;
    public Enemy.Boss3.Boss3Entrance boss3Entrance;
    public GameObject level4Generator;
    public Boss4Entrance boss4Entrance;

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
        GenerateLevel2();
        GenerateLevel3();
        GenerateLevel4();
    }

    public void GenerateLevel1()
    {
        var clonedLevelGeneration = Instantiate(level1Generator);
        var levelGeneration = clonedLevelGeneration.GetComponent<LevelGeneration>();
        levelGeneration.StartGeneration(() =>
        {
            boss1Entrance.MoveBossRoomTo(levelGeneration.pos2.transform.position + Vector3.up * 14 + Vector3.left * 2.5f);
            boss1Entrance.ResetBossRoom();
            Destroy(clonedLevelGeneration);
        });
    }

    public void GenerateLevel2()
    {
        var clonedLevelGeneration = Instantiate(level2Generator);
        var levelGeneration = clonedLevelGeneration.GetComponent<LevelGeneration>();
        levelGeneration.StartGeneration(() =>
        {
            boss2Entrance.MoveBossRoomTo(levelGeneration.pos1.transform.position + Vector3.up * 14 + Vector3.right * 45.01f);
            boss2Entrance.ResetBossRoom();
            Destroy(clonedLevelGeneration);
        });
    }
    public void GenerateLevel3()
    {
        var clonedLevelGeneration = Instantiate(level3Generator);
        var levelGeneration = clonedLevelGeneration.GetComponent<LevelGeneration>();
        levelGeneration.StartGeneration(() =>
        {
            boss3Entrance.MoveBossRoomTo(levelGeneration.pos2.transform.position + Vector3.down * 10f + Vector3.right * 5f);
            boss3Entrance.ResetBossRoom();
            Destroy(clonedLevelGeneration);
        });
    }


    public void GenerateLevel4()
    {
        var clonedLevelGeneration = Instantiate(level4Generator);
        var levelGeneration = clonedLevelGeneration.GetComponent<LevelGeneration>();
        levelGeneration.StartGeneration(() =>
        {
            boss4Entrance.MoveBossRoomTo(levelGeneration.pos2.transform.position + Vector3.up * 30.6f + Vector3.left * 20.01f);
            boss4Entrance.ResetBossRoom();
            Destroy(clonedLevelGeneration);
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(level1Generator.GetComponent<LevelGeneration>().pos2.position, 1);
        Gizmos.DrawWireSphere(level2Generator.GetComponent<LevelGeneration>().pos2.position, 1);
        Gizmos.DrawWireSphere(level3Generator.GetComponent<LevelGeneration>().pos2.position, 1);
        Gizmos.DrawWireSphere(level4Generator.GetComponent<LevelGeneration>().pos2.position, 1);
    }

    [ContextMenu("test")]
    public void OnPlayerRespawn()
    {
        Destroy(generatedGameObject);
        DOVirtual.DelayedCall(.2f, GenerateLevel1);
        DOVirtual.DelayedCall(.2f, GenerateLevel2);
        DOVirtual.DelayedCall(.2f, GenerateLevel3);
        DOVirtual.DelayedCall(.2f, GenerateLevel4);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
