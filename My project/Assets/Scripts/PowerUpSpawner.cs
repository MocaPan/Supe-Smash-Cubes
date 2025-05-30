using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; // Array: assign prefabs for each power-up type in Inspector
    public float spawnInterval = 30f;   // Make it high for rare spawns
    public Transform spawnReferencePoint;
    public Transform leftBound;
    public Transform rightBound;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnPowerUp();
            timer = 0f;
        }
    }

    void SpawnPowerUp()
    {
        // Only spawn if none exist (optional: prevents too many power-ups)
        //if (FindObjectOfType<PowerUpBall>() != null)
            //return;

        // Randomly pick a power-up type
        int idx = Random.Range(0, powerUpPrefabs.Length);

        float spawnX = Random.Range(leftBound.position.x, rightBound.position.x);
        float spawnY = spawnReferencePoint != null ? spawnReferencePoint.position.y : 5f;
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

        Instantiate(powerUpPrefabs[idx], spawnPos, Quaternion.identity);
    }
}
