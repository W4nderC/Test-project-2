using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWinUI : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    void Start()
    {
        continueBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.ContinueGame();
            gameObject.SetActive(false);
        });

        GameManager.Instance.OnGameVictory += GameManager_OnGameVictory;
        gameObject.SetActive(false);
    }

    private void GameManager_OnGameVictory(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
