using UnityEngine;
using TMPro;

public class NumberBall : MonoBehaviour
{
    private int myNumber;

    void Start()
    {
        // Buscar el componente TextMeshPro en los hijos (usualmente en el canvas del prefab)
        TextMeshPro tmp = GetComponentInChildren<TextMeshPro>();

        if (tmp != null && int.TryParse(tmp.text, out int parsed))
        {
            myNumber = parsed;
        }
        else
        {
            Debug.LogWarning($"No se pudo leer el número de la bola en {gameObject.name}.");
        }
    }

    // Método personalizado de colisión, según tu sistema de físicas
    void OnMyCollisionEnter(CustomPhysics2D.MyCollider2D other)
    {
        // Verificar si el otro objeto tiene el componente NumberCollector
        NumberCollector collector = other.GetComponent<NumberCollector>();

        if (collector != null)
        {
            collector.CollectNumber(myNumber); // Pasar el número
            Destroy(gameObject); // Eliminar la bola
        }
    }
}
