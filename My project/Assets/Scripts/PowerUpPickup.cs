using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PowerUpManager.Instance != null)
            {
                // Elegir un poder aleatorio (excepto None)
                PowerUpType randomPower = (PowerUpType)Random.Range(1, System.Enum.GetValues(typeof(PowerUpType)).Length);
                PowerUpManager.Instance.SetPowerUp(randomPower);
            }
            Destroy(gameObject);
        }
    }
} 