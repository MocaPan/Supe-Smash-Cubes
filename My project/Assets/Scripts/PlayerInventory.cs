using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<TreeFragment> fragmentos = new List<TreeFragment>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        TreeFragment fragmento = other.GetComponent<TreeFragment>();
        if (fragmento != null)
        {
            fragmentos.Add(fragmento);
            Destroy(other.gameObject);
            VerificarRetos();
        }
    }

    // Ejemplo de reto: tener 3 fragmentos BST con valores consecutivos
    void VerificarRetos()
    {
        // Filtrar fragmentos BST
        List<TreeFragment> bst = fragmentos.FindAll(f => f.tipo == FragmentType.BST);
        if (bst.Count >= 3)
        {
            // Ordenar por valor
            bst.Sort((a, b) => a.valor.CompareTo(b.valor));
            for (int i = 0; i <= bst.Count - 3; i++)
            {
                if (bst[i + 1].valor == bst[i].valor + 1 && bst[i + 2].valor == bst[i].valor + 2)
                {
                    // ¡Reto completado!
                    if (PowerUpManager.Instance != null)
                        PowerUpManager.Instance.SetPowerUp(PowerUpTypeExtensions.GetRandomPowerUp());
                    // Eliminar los fragmentos usados
                    fragmentos.Remove(bst[i + 2]);
                    fragmentos.Remove(bst[i + 1]);
                    fragmentos.Remove(bst[i]);
                    break;
                }
            }
        }
        // Puedes agregar más retos para AVL, Heap, etc.
    }
} 