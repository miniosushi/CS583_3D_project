using UnityEngine;
using TMPro;

public class EndSceneManager : MonoBehaviour
{
    public TextMeshProUGUI finalTimeText; // Reference to the TextMeshProUGUI element for the final time
    public TextMeshProUGUI finalGiftsText; // Reference to the TextMeshProUGUI element for the final gifts count

    void Start()
    {
        // Display the total time played
        float totalTimePlayed = GiftPickup.GetTotalTimePlayed();
        int minutes = Mathf.FloorToInt(totalTimePlayed / 60F);
        int seconds = Mathf.FloorToInt(totalTimePlayed % 60F);
        finalTimeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);

        // Display the total gifts collected
        int totalGifts = GiftPickup.GetTotalGifts();
        finalGiftsText.text = "Gifts: " + totalGifts;
    }
}
