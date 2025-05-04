using UnityEngine;
using System.Collections;
using CustomPhysics2D;

[RequireComponent(typeof(MyRectangleCollider2D))]
public class KillZone : MonoBehaviour
{
    public AudioSource DeathSound;
    public Transform playerSpawnPoint;
    public float respawnDelay = 1f;

    void OnMyTriggerEnter(MyCollider2D other)
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(RespawnAfterDelay(other.transform));
    }

    private IEnumerator RespawnAfterDelay(Transform player)
    {
        DeathSound?.Play();
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnDelay);
        player.position = playerSpawnPoint.position;
        player.gameObject.SetActive(true);
    }
}
