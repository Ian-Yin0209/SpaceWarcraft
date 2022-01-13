using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    int count;

    // New
    public GameObject[] enemyPools;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 0)
        {
            Instantiate(enemy);
            count = 500;
        }
        count--;
    }
}
