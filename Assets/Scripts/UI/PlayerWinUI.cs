using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWinUI : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private GameObject joystick;

    void Start()
    {
        continueBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.ContinueGame();
            gameObject.SetActive(false);
            joystick.SetActive(true);
        });
        exitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        GameManager.Instance.OnGameVictory += GameManager_OnGameVictory;
        gameObject.SetActive(false);
        joystick.SetActive(true);
    }

    private void GameManager_OnGameVictory(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        joystick.SetActive(false);
    }
}
