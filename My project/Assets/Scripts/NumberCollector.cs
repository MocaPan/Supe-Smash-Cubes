using System.Collections.Generic;
using UnityEngine;

public class NumberCollector : MonoBehaviour
{
    public PlayerTreeHandler playerTreeHandler; // Encargado de manejar el árbol
    public Score myScore;                       // Puntaje de este jugador
    public ChallengeManager challengeManager;   // Referencia global
    public AudioSource audioSource;
    public List<int> collectedNumbers = new List<int>();

    public void CollectNumber(int number)
    {
        // 1. Añadir número a la lista visual/local
        collectedNumbers.Add(number);
        Debug.Log($"Número recogido por {gameObject.name}: {number}");

        // 2. Reproducir sonido (si hay)
        if (audioSource != null)
            audioSource.Play();

        // 3. Insertar número al árbol del jugador
        if (playerTreeHandler != null)
        {
            playerTreeHandler.AddNumber(number);

            // 4. Chequear el reto usando el árbol actualizado
            if (challengeManager != null)
            {
                var myTree = playerTreeHandler.GetTree(); // Asegúrate de que este método lo devuelve
                bool completed = challengeManager.OnPlayerInsertedNumber(myTree, myScore);

                if (completed)
                {
                    Debug.Log($"{gameObject.name} completó el reto: {challengeManager.challengeDescription}");
                    // Aquí puedes activar un power-up, animación, etc.
                }
            }
        }
        else
        {
            Debug.LogWarning("PlayerTreeHandler no está asignado en NumberCollector.");
        }
    }
}
