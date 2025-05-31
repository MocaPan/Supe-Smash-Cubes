using System.Collections.Generic;
using UnityEngine;

public class NumberCollector : MonoBehaviour
{
    public List<int> collectedNumbers = new List<int>();
    public AudioSource audioSource;

    public PlayerTreeHandler playerTreeHandler;

    public void CollectNumber(int number)
    {
        collectedNumbers.Add(number);
        Debug.Log($"Número recogido por {gameObject.name}: {number}");

        if (audioSource != null)
        {
            audioSource.Play();
        }

        
        if (playerTreeHandler != null) // Insertar número en el árbol del jugador
        {
            playerTreeHandler.AddNumber(number);
        }
        else
        {
            Debug.LogWarning("PlayerTreeHandler no está asignado en NumberCollector.");
        }
    }
}
