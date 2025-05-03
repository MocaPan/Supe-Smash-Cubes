using UnityEngine;
using System.Collections;
using CustomPhysics2D;  // donde tengas MyCollider2D y MyRectangleCollider2D

[RequireComponent(typeof(MyRectangleCollider2D))]
public class KillZone : MonoBehaviour
{
    public AudioSource DeathSound;

    public Transform playerSpawnPoint;
    
    public float respawnDelay = 1f;

    bool isRespawning;

    // Este método se invoca porque MyRigidbody2D llama a SendMessage("OnMyTriggerEnter", ...)
    void OnMyTriggerEnter(MyCollider2D other)
    {
        if (isRespawning) return;
        if (!other.CompareTag("Player")) return;
        DeathSound.Play();
        StartCoroutine(RespawnAfterDelay(other.transform));
    }

    IEnumerator RespawnAfterDelay(Transform player)
    {
        isRespawning = true;
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnDelay);
        player.position = playerSpawnPoint.position;
        player.gameObject.SetActive(true);
        isRespawning = false;
    }
}
