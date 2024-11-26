using System.Collections;
using UnityEngine;

public class PresentSpawn : MonoBehaviour
{
    [Header("Spawn settings")]
    public GameObject resourcePrefab; 
    public float spawnChance = 50f; // Percentage chance to spawn at each grid point
    public int maxPresents = 10; // Maximum number of presents in the maze at a time

    [Header("Raycast setup")]
    public float distanceBetweenCheck = 5f; // Grid spacing for spawn points
    public float heightOfCheck = 10f; // Height of the raycast start
    public float rangeOfCheck = 20f; // Distance the raycast checks downwards
    public LayerMask layerMask; // Layer mask for surfaces where presents can spawn
    public Vector2 positivePosition; // Upper-right corner of the spawn area
    public Vector2 negativePosition; // Lower-left corner of the spawn area

    private int currentPresents = 0; // Tracks the number of spawned presents

    private void Start()
    {
        StartCoroutine(SpawnPeriodically());
    }

    private IEnumerator SpawnPeriodically()
    {
        while (true)
        {
            if (currentPresents < maxPresents) // Only spawn if under the max limit
            {
                SpawnResources();
            }
            yield return new WaitForSeconds(Random.Range(5f, 15f)); // Random spawn interval (5-15 seconds)
        }
    }

    private void SpawnResources()
    {
        for (float x = negativePosition.x; x < positivePosition.x; x += distanceBetweenCheck)
        {
            for (float z = negativePosition.y; z < positivePosition.y; z += distanceBetweenCheck)
            {
                if (currentPresents >= maxPresents) return;

                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightOfCheck, z), Vector3.down, out hit, rangeOfCheck, layerMask))
                {
                    if (spawnChance > Random.Range(0f, 100f)) // Check spawn chance
                    {
                        Instantiate(resourcePrefab, hit.point, Quaternion.Euler(0, Random.Range(0, 360), 0), transform);
                        currentPresents++;
                    }
                }
            }
        }
    }

    private void DeleteResources()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
            currentPresents--;
        }
    }

    private void Update()
    {
        // Press 'R' to clear and respawn presents manually
        if (Input.GetKeyDown(KeyCode.R))
        {
            DeleteResources();
            SpawnResources();
        }
    }
}
