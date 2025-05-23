using System;
using UnityEngine;

public class VisualFX : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        GameManager.Instance.OnGameVictory += GameManager_OnGameVictory;
        GameManager.Instance.OnGameDefeat += GameManager_OnGameDefeat;
    }

    private void GameManager_OnGameRestart(object sender, EventArgs e)
    {
        ObjectPoolManager.ReturnObjectToPool(this.gameObject, ObjectPoolManager.PoolType.VisualFX);
    }

    private void GameManager_OnGameVictory(object sender, EventArgs e)
    {
        ObjectPoolManager.ReturnObjectToPool(this.gameObject, ObjectPoolManager.PoolType.VisualFX);
    }

    private void GameManager_OnGameDefeat(object sender, EventArgs e)
    {
        ObjectPoolManager.ReturnObjectToPool(this.gameObject, ObjectPoolManager.PoolType.VisualFX);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
