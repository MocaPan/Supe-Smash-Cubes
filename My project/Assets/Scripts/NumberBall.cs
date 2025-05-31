using UnityEngine;
using TMPro;

public class NumberBall : MonoBehaviour
{
    private int myNumber;

    void Start()
    {
        
        TextMeshPro tmp = GetComponentInChildren<TextMeshPro>();// Obtener el número desde el TextMeshPro
        if (tmp != null && int.TryParse(tmp.text, out int parsed))
        {
            myNumber = parsed;
        }
        else
        {
            Debug.LogWarning("No se pudo leer el número de la bola.");
        }
    }

    void OnMyCollisionEnter(CustomPhysics2D.MyCollider2D other)
    {
        
        NumberCollector collector = other.GetComponent<NumberCollector>();// Verifica si colisiona con el jugador
        if (collector != null)
        {
            collector.CollectNumber(myNumber);
            Destroy(gameObject); // Destruye la bola
        }
    }
}
