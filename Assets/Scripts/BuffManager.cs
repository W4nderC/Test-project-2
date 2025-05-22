using UnityEngine;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }

    public BuffSO[] buffSos;

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


    // public void ApplyBuff(BuffSO buffData)
    // {
    //     Buff newBuff = new Buff(buffData);
    //     activeBuffs.Add(newBuff);
    //     Debug.Log("Applied buff: " + buffData.buffType);
    //     // Tuỳ buffType, bạn có thể áp dụng hiệu ứng tại đây
    // }

    // void Update()
    // {
    // }

    // public bool HasBuff(BuffType type)
    // {
    //     return activeBuffs.Exists(b => b.data.buffType == type);
    // }
}
