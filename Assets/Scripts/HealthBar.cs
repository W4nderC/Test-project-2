using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBarImg;
    
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _healthBarImg.fillAmount = currentHealth / maxHealth;
    }

    private void Update()
    {
        transform.LookAt(_mainCamera.transform);
    }
}
