using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartGameUI : MonoBehaviour
{
    public TMP_Dropdown dropdown; 
    public Button startButton; 
    [SerializeField] private GameObject joystick;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            GameManager.Instance.GameStart();
            gameObject.SetActive(false);
            joystick.SetActive(true);
        });
    }

    void Start()
    {
        gameObject.SetActive(true);

        if (dropdown != null)
        {
            dropdown.options.Clear();

            // Thêm các giá trị Enum vào dropdown
            foreach (string modeName in System.Enum.GetNames(typeof(GameMode)))
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(modeName));
            }

            dropdown.value = 0;
            dropdown.RefreshShownValue();
        }
        
        GameManager.Instance.OnGameWaitingToStart += GameManager_OnGameWaitingToStart;
    }

    private void GameManager_OnGameWaitingToStart(object sender, EventArgs e)
    {
        gameObject.SetActive(true);

    }

    public void GetDropdownValue()
    {
        int pickedIndex = dropdown.value;
        // string selectOption = dropdown.options[pickedIndex].text;
        GameManager.Instance.SetMode((GameMode)pickedIndex);
    }
}
