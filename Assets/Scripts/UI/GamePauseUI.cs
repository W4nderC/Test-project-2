using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button homeBtn;
    [SerializeField] private GameObject joystick;

    private void Awake()
    {
        resumeBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });
        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        homeBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.InvokeGameWaitingToStart();
            gameObject.SetActive(false);
            joystick.SetActive(false);
            GameManager.Instance.TogglePauseGame();
        });
    }

    void Start()
    {
        GameManager.Instance.OnLocalGamePaused += GameManager_OnLocalGamePaused;
        GameManager.Instance.OnLocalGameUnpaused += GameManager_OnLocalGameUnpaused;

        gameObject.SetActive(false);
    }

    private void GameManager_OnLocalGameUnpaused(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
        joystick.SetActive(true);
    }

    private void GameManager_OnLocalGamePaused(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
        joystick.SetActive(false);
    }

}
