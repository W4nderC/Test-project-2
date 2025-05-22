using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private Button pauseBtn;

    void Awake ()
    {
        pauseBtn.onClick.AddListener(()=>
        {
            GameManager.Instance.TogglePauseGame();
        });
    }

    public void UpdateLevelText()
    {
        currentLevelText.text = "Level: " + LevelGeneratorManager.currentLevel.ToString();
    }   
}
