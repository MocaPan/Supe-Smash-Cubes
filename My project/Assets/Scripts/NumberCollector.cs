﻿using System.Collections.Generic;
using UnityEngine;

public class NumberCollector : MonoBehaviour
{
    public List<int> collectedNumbers = new List<int>();
    public AudioSource audioSource;

    public TreeManager treeManager; // ← Añade esta referencia

    public void CollectNumber(int number)
    {
        collectedNumbers.Add(number);
        Debug.Log($"Número recogido por {gameObject.name}: {number}");

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (treeManager != null)
        {
            treeManager.InsertNumber(number); // ← Aquí se inserta en el árbol y se visualiza
        }
    }
}
