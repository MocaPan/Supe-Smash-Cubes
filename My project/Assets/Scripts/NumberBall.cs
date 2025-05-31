using UnityEngine;
using TMPro;

public class NumberBall : MonoBehaviour
{
    private int myNumber;

    void Start()
    {
        
        TextMeshPro tmp = GetComponentInChildren<TextMeshPro>();// Obtener el n�mero desde el TextMeshPro
        if (tmp != null && int.TryParse(tmp.text, out int parsed))
        {
            myNumber = parsed;
        }
        else
        {
            Debug.LogWarning("No se pudo leer el n�mero de la bola.");
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
