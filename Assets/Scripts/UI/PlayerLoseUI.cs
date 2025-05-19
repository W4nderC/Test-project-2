using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoseUI : MonoBehaviour
{
    [SerializeField] private Button retryBtn;
    void Start()
    {
        retryBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartGame();
            gameObject.SetActive(false);
        });

        GameManager.Instance.OnGameDefeat += GameManager_OnGameDefeat;
        gameObject.SetActive(false);
    }

    private void GameManager_OnGameDefeat(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
