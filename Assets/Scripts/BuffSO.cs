using UnityEngine;

public enum BuffType
{
    IncreaseAttack,
    IncreaseHealth,
    IncreaseAtkSpeed,
    HealOverTime,
}

[CreateAssetMenu(fileName = "BuffSO", menuName = "ScriptableObjects/BuffSO")]
public class BuffSO : ScriptableObject
{
    public string buffName;
    public BuffType buffType;
    public int atk;
    public int health;
    public float atkSpeed;
    public float healOverTime;
    public GameObject buffPrefab;
    public string description;

    public void ApplyBuffStat
    (
        Transform pos, int damage, int maxHealth, int currentHealth, float baseAtkSpeed,
        out int newDmg, out int newMaxHP, out int newCurHP, out float newAtkSpd,
        bool buffSpawnVisual = true
    )
    {
        newDmg = damage + atk;
        newMaxHP = maxHealth + health;
        newAtkSpd = baseAtkSpeed - atkSpeed * .05f;
        newCurHP = currentHealth + 1;

        if (buffSpawnVisual)
        {
            //Instantiate buff visual
            GameObject buffVisual = Instantiate(buffPrefab, pos.position, Quaternion.identity);
            buffVisual.transform.SetParent(pos);
        }
    }
// public struct BuffStats
// {
//     public int damage;
//     public int maxHealth;
//     public int currentHealth;
//     public float baseAtkSpeed;
// }  
// public BuffStats BuffStat(Transform pos, int damage, int maxHealth, int currentHealth, float baseAtkSpeed)
// {
//     BuffStats result;
//     result.damage = damage + atk;
//     result.maxHealth = maxHealth + health;
//     result.currentHealth = currentHealth + 1;
//     result.baseAtkSpeed = baseAtkSpeed - atkSpeed * 0.05f;

//     // Instantiate buff visual
//     GameObject buffVisual = Instantiate(buffPrefab, pos.position, Quaternion.identity);
//     buffVisual.transform.SetParent(pos);

//     return result;
// }
}
