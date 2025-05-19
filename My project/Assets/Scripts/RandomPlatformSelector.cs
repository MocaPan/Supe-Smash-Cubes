using UnityEngine;

public class RandomPlatformSelector : MonoBehaviour
{
    [Tooltip("Lista con todas las plataformas posibles")]
    public GameObject[] plataformas;

    [Tooltip("Cantidad mínima de plataformas que se mostrarán")]
    public int minPlataformas = 1;

    [Tooltip("Cantidad máxima de plataformas que se mostrarán")]
    public int maxPlataformas = 5;

    void Start()
    {
        if (plataformas == null || plataformas.Length == 0)
        {
            Debug.LogWarning("No hay plataformas asignadas.");
            return;
        }

        // Validar límites
        int min = Mathf.Clamp(minPlataformas, 0, plataformas.Length);
        int max = Mathf.Clamp(maxPlataformas, min, plataformas.Length);

        int cantidad = Random.Range(min, max + 1);

        // Primero desactivar todas
        foreach (var plat in plataformas)
        {
            plat.SetActive(false);
        }

        // Elegir 'cantidad' plataformas únicas al azar para activar
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
