using UnityEngine;

public class RandomPlatformSelector : MonoBehaviour
{
    [Tooltip("Lista con todas las plataformas posibles")]
    public GameObject[] plataformas;

    [Tooltip("Cantidad m�nima de plataformas que se mostrar�n")]
    public int minPlataformas = 1;

    [Tooltip("Cantidad m�xima de plataformas que se mostrar�n")]
    public int maxPlataformas = 5;

    void Start()
    {
        if (plataformas == null || plataformas.Length == 0)
        {
            Debug.LogWarning("No hay plataformas asignadas.");
            return;
        }

        // Validar l�mites
        int min = Mathf.Clamp(minPlataformas, 0, plataformas.Length);
        int max = Mathf.Clamp(maxPlataformas, min, plataformas.Length);

        int cantidad = Random.Range(min, max + 1);

        // Primero desactivar todas
        foreach (var plat in plataformas)
        {
            plat.SetActive(false);
        }

        // Elegir 'cantidad' plataformas �nicas al azar para activar
        for (int i = 0; i < cantidad; i++)
        {
            int index;
            do
            {
                index = Random.Range(0, plataformas.Length);
            }
            while (plataformas[index].activeSelf); // asegurar no repetir

            plataformas[index].SetActive(true);
        }
    }
}
