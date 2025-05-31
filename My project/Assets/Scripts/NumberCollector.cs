using System.Collections.Generic;
using UnityEngine;

public class NumberCollector : MonoBehaviour
{
    public int playerId = 1; // Asigna 1 o 2 en el Inspector según el jugador
    public List<int> collectedNumbers = new List<int>();
    public AudioSource audioSource;

    public TreeManager treeManager; // Arrástralo en el Inspector (TreeManagerObject)

    public void CollectNumber(int number)
    {
        collectedNumbers.Add(number);
        Debug.Log($"Número recogido por Jugador {playerId}: {number}");

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (treeManager != null)
        {
            treeManager.InsertNumberForPlayer(number, playerId); // Llama al árbol correcto
        }
        else
        {
            Debug.LogWarning("TreeManager no asignado en NumberCollector");
        }
    }
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



