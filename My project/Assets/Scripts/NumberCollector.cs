using System.Collections.Generic;
using UnityEngine;

public class NumberCollector : MonoBehaviour
{
    public IProgrammingTree<int> myTree;      // Asigna en el Inspector o por código
    public Score myScore;                // Asigna en el Inspector o por código
    public ChallengeManager challengeManager; // Referencia global
    public List<int> collectedNumbers = new List<int>();
    public AudioSource audioSource;

    public PlayerTreeHandler playerTreeHandler;

    public void CollectNumber(int number)
    {
        if (playerTreeHandler != null)
        {
            playerTreeHandler.AddNumber(number);
        }
        else
        {
            Debug.LogWarning("PlayerTreeHandler no está asignado en NumberCollector.");
            return;
        }

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

        // Si necesitas checar el reto, descomenta y adapta esta sección:
        /*
        if (challengeManager != null)
        {
            bool completed = challengeManager.OnPlayerInsertedNumber(myTree, myScore);

            if (completed)
            {
                Debug.Log($"{gameObject.name} completó el reto: {challengeManager.challengeDescription}");
                // Aquí podrías activar un power-up, mostrar un mensaje, etc.
            }
        }
        */
    }
}

