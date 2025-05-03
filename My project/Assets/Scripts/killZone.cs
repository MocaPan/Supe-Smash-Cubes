using UnityEngine;
using System.Collections;


public class KillZone2D : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public float respawnDelay = 2f;
    private bool isRespawning = false;
    public AudioSource RespawnSound;
    


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isRespawning && other.CompareTag("Player"))
        {
            RespawnSound.Play();
            StartCoroutine(RespawnAfterDelay(other.gameObject));
        }
    }

    private IEnumerator RespawnAfterDelay(GameObject player)
    {
        isRespawning = true;
        player.SetActive(false);
        yield return new WaitForSeconds(respawnDelay);
        player.transform.position = playerSpawnPoint.position;
        player.SetActive(true);
        isRespawning = false;
    }
}
