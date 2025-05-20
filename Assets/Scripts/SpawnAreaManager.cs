using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaManager : MonoBehaviour
{
    public static SpawnAreaManager Instance;

    [SerializeField] private Collider spawnAreaCollider;
    [SerializeField] private LayerMask _layerCannotSpawnOn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnCharactor(GameObject charactorPrefab, Collider areaSpawnCol, ObjectPoolManager.PoolType poolType = ObjectPoolManager.PoolType.GameObjects)
    {
        ObjectPoolManager.SpawnObject(charactorPrefab, GetRandomSpawnPosition(areaSpawnCol), Quaternion.identity, poolType);
    }

    private Vector3 GetRandomSpawnPosition(Collider spawnableAreaCollider)
    {
        Vector3 spawnPosition = Vector3.zero;
        bool isSpawnPosValid = false;

        int attemptCount = 0;
        int maxAttempts = 200;


        while (!isSpawnPosValid && attemptCount < maxAttempts)
        {
            spawnPosition = GetRandomPointInCollider(spawnableAreaCollider);
            Collider[] colliders = Physics.OverlapSphere(spawnPosition, 1f);

            bool isInValidCollision = false;
            foreach (Collider collider in colliders)
            {
                if(((1 << collider.gameObject.layer) & _layerCannotSpawnOn) != 0)
                {
                    isInValidCollision = true;
                    break;
                }
            }

            if (!isInValidCollision)
            {
                isSpawnPosValid = true;
            }

            attemptCount++;
        }

        if (!isSpawnPosValid)
        {
            Debug.LogWarning("Failed to find a valid spawn position");
            return Vector3.zero;
        }

        return spawnPosition;
    }

    private Vector3 GetRandomPointInCollider(Collider collider, float offset = 1f)
    {
        Bounds collBounds = collider.bounds;

        Vector3 minBounds = new Vector3(collBounds.min.x + offset, collBounds.min.y, collBounds.min.z + offset);
        Vector3 maxBounds = new Vector3(collBounds.max.x - offset, collBounds.max.y, collBounds.max.z - offset);

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomZ = Random.Range(minBounds.z, maxBounds.z);

        return new Vector3(randomX, collBounds.min.y, randomZ);
    }
}
