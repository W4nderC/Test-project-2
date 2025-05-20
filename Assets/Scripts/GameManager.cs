using System;
using UnityEngine;

public enum GameMode { OneVsOne, OneVsMany, ManyVsMany }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnGameVictory;
    public event EventHandler OnGameDefeat;
    public event EventHandler OnGameRestart;
    public event EventHandler OnGameContinue;

    public GameMode currentMode;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject allyPrefab;
    public Transform[] spawnPoints;

    [SerializeField] private Collider playerSide;
    [SerializeField] private Collider enemySide;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        OnGameRestart += GameManager_OnGameRestart;

        GameStart();
    }

    private void GameManager_OnGameRestart(object sender, EventArgs e)
    {
        GameStart();
    }

    private void GameStart()
    {
        switch (currentMode)
        {
            case GameMode.OneVsOne:
                ObjectPoolManager.SpawnObject(playerPrefab, spawnPoints[0].position, Quaternion.identity, ObjectPoolManager.PoolType.Player);
                ObjectPoolManager.SpawnObject(enemyPrefab, spawnPoints[1].position, Quaternion.identity, ObjectPoolManager.PoolType.Enemy);
                break;

            case GameMode.OneVsMany:
                ObjectPoolManager.SpawnObject(playerPrefab, spawnPoints[0].position, Quaternion.identity, ObjectPoolManager.PoolType.Player);
                for (int i = 1; i < 5; i++)
                    SpawnAreaManager.Instance.SpawnCharactor(enemyPrefab, enemySide, ObjectPoolManager.PoolType.Enemy);
                break;

            case GameMode.ManyVsMany:
                ObjectPoolManager.SpawnObject(playerPrefab, spawnPoints[0].position, Quaternion.identity, ObjectPoolManager.PoolType.Player);
                for (int i = 0; i < 3; i++)
                    SpawnAreaManager.Instance.SpawnCharactor(allyPrefab, playerSide, ObjectPoolManager.PoolType.Ally);
                for (int i = 0; i < 4; i++)
                    SpawnAreaManager.Instance.SpawnCharactor(enemyPrefab, enemySide, ObjectPoolManager.PoolType.Enemy);
                break;
        }
        GameObject player = FindObjectOfType<PlayerController>().gameObject;
        CameraControl.SetCameraFollow(player.transform);
    }

    public void RestartGame()
    {
        OnGameRestart?.Invoke(this, EventArgs.Empty);
    }

    public void PlayerWin()
    {
        OnGameVictory?.Invoke(this, EventArgs.Empty);
    }

    public void PlayerLose()
    {
        OnGameDefeat?.Invoke(this, EventArgs.Empty);
    }

    public void ContinueGame()
    {
        OnGameContinue?.Invoke(this, EventArgs.Empty); print("Continue Game");
        GameStart();
    }
}
