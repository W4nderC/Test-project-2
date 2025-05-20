using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoseUI : MonoBehaviour
{
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private GameObject joystick;

    void Start()
    {
        retryBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartGame();
            gameObject.SetActive(false);
            joystick.SetActive(true);
        });
        exitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        GameManager.Instance.OnGameDefeat += GameManager_OnGameDefeat;
        gameObject.SetActive(false);
        joystick.SetActive(true);
    }

    private void GameManager_OnGameDefeat(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        joystick.SetActive(false);
    }
}
