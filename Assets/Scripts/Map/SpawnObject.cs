using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, objects.Length);
        GameObject go = Instantiate(objects[rand], transform.position, Quaternion.identity);
        GameManager.Instance.SetParentToGenerated(go);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
