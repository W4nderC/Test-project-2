using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ObjectPooler : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int poolSize = 50;
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetFromPool()
    {
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        pool.Enqueue(obj);
        return obj;
    }
}
