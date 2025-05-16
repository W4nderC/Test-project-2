using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{

    public int numberOfLevels = 10;
    public List<LevelData> levels = new List<LevelData>();

    void Start()
    {
        for (int i = 1; i <= numberOfLevels; i++)
        {
            LevelData level = new LevelData();
            level.enemyCount = i * 2; // tăng số lượng enemy
            level.enemyHealth = 50 + i * 10; // enemy khỏe hơn
            level.enemyDamage = 5 + i; // enemy đánh mạnh hơn
            levels.Add(level);
        }
    }

}

[System.Serializable]
public class LevelData
{
    public int enemyCount;
    public int enemyHealth;
    public int enemyDamage;
}
