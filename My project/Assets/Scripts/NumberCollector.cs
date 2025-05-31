using System.Collections.Generic;
using UnityEngine;

public class NumberCollector : MonoBehaviour
{
    public IProgrammingTree myTree;      // Asigna en el Inspector o por código
    public Score myScore;                // Asigna en el Inspector o por código
    public ChallengeManager challengeManager; // Referencia global
    public List<int> collectedNumbers = new List<int>();
    public AudioSource audioSource;

    public void CollectNumber(int number)
    {
        myTree.Insert(number);
        collectedNumbers.Add(number);
        Debug.Log($"N�mero recogido por {gameObject.name}: {number}");

        if (audioSource != null)
        {
            audioSource.Play();
        }
        // 4. Revisa si el reto está cumplido
        if (challengeManager != null)
        {
            bool completed = challengeManager.OnPlayerInsertedNumber(myTree, myScore);

            // Si el reto se cumplió, podrías dar feedback visual/auditivo aquí
            if (completed)
            {
                Debug.Log($"{gameObject.name} completó el reto: {challengeManager.challengeDescription}");
                // Aquí podrías activar un power-up, mostrar un mensaje, etc.
            }
        }
    }
    // Cuando recolectas un número:
    public void OnNumberCollected(int value)
    {
        myTree.Insert(value);
        if (challengeManager.currentChallenge(myTree))
        {
            // El jugador completó el reto: ¡gana puntos y sigue el juego!
            Debug.Log("¡Reto cumplido!");
            // challengeManager.NextChallenge()...
        }
    }

}
