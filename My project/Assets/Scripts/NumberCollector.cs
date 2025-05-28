using System.Collections.Generic;
using UnityEngine;

public class NumberCollector : MonoBehaviour
{
    public List<int> collectedNumbers = new List<int>();
    public AudioSource audioSource;

    public void CollectNumber(int number)
    {
        collectedNumbers.Add(number);
        Debug.Log($"N�mero recogido por {gameObject.name}: {number}");

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
