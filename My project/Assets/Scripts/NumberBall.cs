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
            Debug.LogWarning($"No se pudo leer el n�mero de la bola en {gameObject.name}.");
        }
    }

    // M�todo personalizado de colisi�n, seg�n tu sistema de f�sicas
    void OnMyCollisionEnter(CustomPhysics2D.MyCollider2D other)
    {
        // Verificar si el otro objeto tiene el componente NumberCollector
        NumberCollector collector = other.GetComponent<NumberCollector>();

        if (collector != null)
        {
            collector.CollectNumber(myNumber); // Pasar el n�mero
            Destroy(gameObject); // Eliminar la bola
        }
    }
}
