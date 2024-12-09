using UnityEngine;
using UnityEngine.SceneManagement; // For restarting the level
using UnityEngine.UI; // For displaying the game over screen

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the player
    public Image healthBar; // Reference to the health bar UI element
    public GameObject gameOverScreen; // Reference to the game over screen UI element
    private float currentHealth; // Current health of the player

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health
        UpdateHealthBar(); // Initialize the health bar
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false); // Hide the game over screen at the start
        }
    }

    // Function to take damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Reduce current health by the damage amount
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        UpdateHealthBar(); // Update the health bar

        if (currentHealth <= 0)
        {
            Die(); // Call the Die function if health is 0 or less
        }
    }

    // Function to handle player death
    private void Die()
    {
        Debug.Log("Player has died.");
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true); // Show the game over screen
        }
        // Optionally, restart the level after a delay
        Invoke("RestartLevel", 3f); // Restart the level after 3 seconds
    }

    // Function to restart the level
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    // Function to update the health bar
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth; // Update the fill amount
        }
    }
}
