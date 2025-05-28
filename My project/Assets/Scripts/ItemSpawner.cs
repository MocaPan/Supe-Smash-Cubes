using UnityEngine;
using TMPro;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public float spawnInterval = 20f;
    public Transform spawnReferencePoint;

    public Transform leftBound; 
    public Transform rightBound; 

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnItem();
            timer = 0f;
        }
    }

    void SpawnItem()
    {
        int number = Random.Range(1, 16);

        
        float spawnX = Random.Range(leftBound.position.x, rightBound.position.x);
        float spawnY = spawnReferencePoint != null ? spawnReferencePoint.position.y : 5f;
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

        GameObject newItem = Instantiate(itemPrefab, spawnPos, Quaternion.identity);

        
        TextMeshPro tmp = newItem.GetComponentInChildren<TextMeshPro>();// Asignar número al texto
        if (tmp != null)
        {
            tmp.text = number.ToString();
            tmp.color = Color.black;
            tmp.sortingOrder = 10;
        }
        else
        {
            Debug.LogWarning("El prefab no tiene un TextMeshPro hijo.");
        }

        // Física personalizada
        CustomPhysics2D.MyRigidbody2D rb = newItem.GetComponent<CustomPhysics2D.MyRigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1.0f;
            rb.linearVelocity = Vector2.zero;
        }
    }
}
