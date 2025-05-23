using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private bool _addToDontDestroyOnLoad = false;

    private GameObject _emptyHolder;

    private static GameObject _particleSystemsEmpty;
    private static GameObject _gameObjectsEmpty;
    private static GameObject _soundFXEmpty;

    // Custom
    private static GameObject _playerEmpty;
    private static GameObject _enemyEmpty;
    private static GameObject _allyEmpty;
    private static GameObject _visualFXEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;
    private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

    public enum PoolType
    {
        ParticleSystem,
        GameObjects,
        SoundFX,

        // Custom
        Player,
        Enemy,
        Ally,

        VisualFX,
    }
    public static PoolType PoolingType;

    private void Awake()
    {
        _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();

        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _emptyHolder = new GameObject("Object Pools");

        _particleSystemsEmpty = new GameObject("Particle Effects");
        _gameObjectsEmpty = new GameObject("Game Objects");
        _soundFXEmpty = new GameObject("Sound FX");

        _particleSystemsEmpty.transform.SetParent(_emptyHolder.transform);
        _gameObjectsEmpty.transform.SetParent(_emptyHolder.transform);
        _soundFXEmpty.transform.SetParent(_emptyHolder.transform);


        // Add custom
        _playerEmpty = new GameObject("Player");
        _enemyEmpty = new GameObject("Enemy");
        _allyEmpty = new GameObject("Ally");
        _visualFXEmpty = new GameObject("Visual FX");

        _playerEmpty.transform.SetParent(_emptyHolder.transform);
        _enemyEmpty.transform.SetParent(_emptyHolder.transform);
        _allyEmpty.transform.SetParent(_emptyHolder.transform);
        _visualFXEmpty.transform.SetParent(_emptyHolder.transform);

        if (_addToDontDestroyOnLoad)
            DontDestroyOnLoad(_particleSystemsEmpty.transform.root);
    }

    private static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, pos, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );

        _objectPools.Add(prefab, pool);
    }

    private static GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
    {
        prefab.SetActive(false);

        GameObject obj = Instantiate(prefab, pos, rot);

        prefab.SetActive(true);

        GameObject parentObject = SetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform);

        return obj;
    }

    private static void OnGetObject(GameObject obj)
    {

    }

    private static void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    private static void OnDestroyObject(GameObject obj)
    {
        if (_cloneToPrefabMap.ContainsKey(obj))
        {
            _cloneToPrefabMap.Remove(obj);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.ParticleSystem:
                return _particleSystemsEmpty;
            case PoolType.GameObjects:
                return _gameObjectsEmpty;
            case PoolType.SoundFX:
                return _soundFXEmpty;

            // Custom
            case PoolType.Player:
                return _playerEmpty;
            case PoolType.Enemy:
                return _enemyEmpty;
            case PoolType.Ally:
                return _allyEmpty;

            case PoolType.VisualFX:
                return _visualFXEmpty;

            default:
                return null;
        }
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Object
    {
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, spawnPos, spawnRotation, poolType);
        }

        GameObject obj = _objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            obj.transform.position = spawnPos;
            obj.transform.rotation = spawnRotation;
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }

            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"Object {objectToSpawn.name} does not have a component of type {typeof(T)}.");
                return null;
            }

            return component;
        }

        return null;
    }

    public static T SpawnObject<T>(T typePrefab, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, spawnPos, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects)
    {
        return SpawnObject<GameObject>(objectToSpawn, spawnPos, spawnRotation, poolType);
    }

    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObjects)
    {
        if (_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);

            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }

            if (_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else
        {
            Debug.LogWarning("Trying to return an object that is not in the pool." + obj.name);
        }
    }

    // Add Custom code
    public static bool CheckPoolChildInactive(PoolType poolType = PoolType.GameObjects)
    {
        GameObject parentObject = SetParentObject(poolType);

        int poolSize = parentObject.transform.childCount;
        int inactiveCount = 0;
        foreach (Transform child in parentObject.transform)
        {
            if (!child.gameObject.activeSelf)
            {
                inactiveCount++;
            }
        }

        return poolSize == inactiveCount;
    }
}
