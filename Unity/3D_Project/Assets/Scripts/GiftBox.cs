using UnityEngine;
using TMPro; // Make sure to include the TextMeshPro namespace

public class GiftBox : MonoBehaviour
{
    public int giftValue = 1; // Amount of gifts this box provides
    public TextMeshProUGUI giftText; // Reference to the TextMeshProUGUI element
    private static int totalGifts = 0; // Total gifts collected by the player
    private bool isPlayerInRange = false; // Track if the player is in range

    void Update()
    {
        // Check for player input if they are in range
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpGift();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger zone
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void PickUpGift()
    {
        totalGifts += giftValue; // Update the total gifts
        UpdateGiftText(); // Update the UI text
        Destroy(gameObject); // Destroy the gift box after pickup
    }

    private void UpdateGiftText()
    {
        if (giftText != null)
        {
            giftText.text = "Gifts: " + totalGifts; // Update the displayed gift count
        }
    }
}