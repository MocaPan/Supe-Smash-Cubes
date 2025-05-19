using UnityEngine;
using UnityEngine.Tilemaps;

namespace CustomPhysics2D
{

    // Colisionador personalizado para Tilemap: cada celda con tile se considera un bloque sólido.
    public class MyTilemapCollider2D : MyCollider2D
    {
        private Tilemap tilemap;

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
            if (tilemap == null)
            {
                Debug.LogError("MyTilemapCollider2D requiere un Tilemap en el mismo GameObject.");
            }
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (tilemap == null)
            {
                // Intentar obtener referencia al vuelo si no está inicializada (ej. en editor)
                tilemap = GetComponent<Tilemap>();
                if (tilemap == null) return;
            }
            // Iterar por todas las posiciones de celda dentro de los límites del tilemap
            BoundsInt bounds = tilemap.cellBounds;
            Vector3 cellSize = tilemap.cellSize;
            foreach (var pos in bounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos)) continue;
                // Obtener la posición del bloque en el mundo. Usamos el centro de la celda para dibujar el cuadro.
                Vector3 worldPos = tilemap.CellToWorld(pos) + (Vector3)cellSize / 2;
                // Dibujar un cubo alambre representando el bloque
                Gizmos.DrawWireCube(worldPos, cellSize);
            }
        }
    }
}