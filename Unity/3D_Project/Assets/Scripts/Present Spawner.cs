using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections.Generic;

public class PresentSpawner : MonoBehaviour
{
    public GameObject[] presents; // Array of present prefabs
    public int numberOfPresents = 10; // Number of presents to spawn
    public float spawnRadius = 50f; // Radius within which to spawn presents
    public float minDistanceBetweenPresents = 5f; // Minimum distance between presents
    public TextMeshProUGUI giftText; // Reference to the TextMeshProUGUI element

    private List<Vector3> spawnPositions = new List<Vector3>(); // List to store spawn positions

    void Start()
    {
        SpawnPresents();
    }

    void SpawnPresents()
    {
        for (int i = 0; i < numberOfPresents; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                int randomIndex = Random.Range(0, presents.Length);
                GameObject present = Instantiate(presents[randomIndex], spawnPosition, Quaternion.identity);
                GiftBox giftBox = present.GetComponent<GiftBox>();
                if (giftBox != null)
                {
                    giftBox.giftText = giftText; // Set the giftText reference
                }
                spawnPositions.Add(spawnPosition); // Add the position to the list
            }
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPosition;
        int attempts = 0;
        do
        {
            spawnPosition = GetRandomNavMeshPosition();
            attempts++;
        } while (!IsPositionValid(spawnPosition) && attempts < 100); // Limit attempts to avoid infinite loop

        return spawnPosition;
    }

    Vector3 GetRandomNavMeshPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero;
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 existingPosition in spawnPositions)
        {
            if (Vector3.Distance(position, existingPosition) < minDistanceBetweenPresents)
            {
                return false;
            }
        }
        return true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
