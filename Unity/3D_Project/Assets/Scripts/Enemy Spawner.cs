using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies; // Array of enemy prefabs
    public int numberOfEnemies = 10; // Number of enemies to spawn
    public float spawnRadius = 50f; // Radius within which to spawn enemies
    public float minDistanceBetweenEnemies = 5f; // Minimum distance between enemies

    private List<Vector3> spawnPositions = new List<Vector3>(); // List to store spawn positions

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                int randomIndex = Random.Range(0, enemies.Length);
                GameObject enemy = Instantiate(enemies[randomIndex], spawnPosition, Quaternion.identity);
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
            if (Vector3.Distance(position, existingPosition) < minDistanceBetweenEnemies)
            {
                return false;
            }
        }
        return true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
