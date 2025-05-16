using UnityEngine;

public enum GameMode { OneVsOne, OneVsMany, ManyVsMany }

public class GameManager : MonoBehaviour
{
    public GameMode currentMode;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    void Start()
    {
        switch (currentMode)
        {
            case GameMode.OneVsOne:
                Instantiate(playerPrefab, spawnPoints[0].position, Quaternion.identity);
                Instantiate(enemyPrefab, spawnPoints[1].position, Quaternion.identity);
                break;
            case GameMode.OneVsMany:
                Instantiate(playerPrefab, spawnPoints[0].position, Quaternion.identity);
                for (int i = 1; i < spawnPoints.Length; i++)
                    Instantiate(enemyPrefab, spawnPoints[i].position, Quaternion.identity);
                break;
            case GameMode.ManyVsMany:
                for (int i = 0; i < spawnPoints.Length / 2; i++)
                    Instantiate(playerPrefab, spawnPoints[i].position, Quaternion.identity);
                for (int i = spawnPoints.Length / 2; i < spawnPoints.Length; i++)
                    Instantiate(enemyPrefab, spawnPoints[i].position, Quaternion.identity);
                break;
        }
    }
}
