using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    public Slider healthBarSlider; // Reference to the Health Bar Slider
    public GameObject gameOverPanel; // Reference to the Game Over Panel

    void Start()
    {
        currentHealth = maxHealth;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;
        gameOverPanel.SetActive(false); // Make sure the Game Over Panel is hidden at the start
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthBar();
        
        if (currentHealth == 0)
        {
            ShowGameOver();
        }
    }

    void UpdateHealthBar()
    {
        healthBarSlider.value = currentHealth;
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true); // Show the Game Over Panel
    }
}
