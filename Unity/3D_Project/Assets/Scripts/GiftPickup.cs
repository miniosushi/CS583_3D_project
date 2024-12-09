using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GiftPickup : MonoBehaviour
{
    public int giftValue = 1; // Amount of gifts each box provides
    public TextMeshProUGUI giftText; // Reference to the TextMeshProUGUI element
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI element for the timer
    static int totalGifts = 0; // Total gifts collected by the player
    static float totalTimePlayed = 0f; // Total time played

    private bool isGameWon = false; // Track if the game is won

    void Update()
    {
        // Update the timer if the game is not won
        if (!isGameWon)
        {
            totalTimePlayed += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with a gift box
        if (other.CompareTag("Gift"))
        {
            PickUpGift(other.gameObject);
        }
        // Win Game if Touch Goal
        if (other.CompareTag("WinGame"))
        {
            isGameWon = true;
            SceneManager.LoadScene("EndScene");
        }
    }

    private void PickUpGift(GameObject giftBox)
    {
        totalGifts += giftValue; // Update the total gifts
        UpdateGiftText(); // Update the UI text
        Destroy(giftBox); // Destroy the gift box after pickup
    }

    private void UpdateGiftText()
    {
        if (giftText != null)
        {
            giftText.text = "Gifts: " + totalGifts; // Update the displayed gift count
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(totalTimePlayed / 60F);
            int seconds = Mathf.FloorToInt(totalTimePlayed % 60F);
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds); // Update the displayed time
        }
    }

    public bool HasGifts()
    {
        return totalGifts > 0;
    }

    public void UseGift()
    {
        if (totalGifts > 0)
        {
            totalGifts--;
            UpdateGiftText(); // Update the UI text after using a gift
        }
    }

    public static float GetTotalTimePlayed()
    {
        return totalTimePlayed;
    }

    public static int GetTotalGifts()
    {
        return totalGifts;
    }
    public static void ResetTimer()
    {
        totalTimePlayed = 0f;
    }
}