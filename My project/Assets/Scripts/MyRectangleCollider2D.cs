using UnityEngine;

namespace CustomPhysics2D
{
    // Colisionador rectangular 2D personalizado
    public class MyRectangleCollider2D : MyCollider2D
    {
        // Dimensiones del rect�ngulo (ancho y alto totales)
        public float width = 1f;
        public float height = 1f;

        // Propiedades calculadas para facilitar c�lculos (medias dimensiones)
        public Vector2 HalfSize
        {
            get { return new Vector2(width / 2f, height / 2f); }
        }

        // Dibuja el rect�ngulo en la vista de Escena usando Gizmos (como wireframe)
        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector2 half = HalfSize;
            // Esquinas del rect�ngulo
            Vector3 topLeft = transform.position + new Vector3(-half.x, half.y, 0);
            Vector3 topRight = transform.position + new Vector3(half.x, half.y, 0);
            Vector3 bottomLeft = transform.position + new Vector3(-half.x, -half.y, 0);
            Vector3 bottomRight = transform.position + new Vector3(half.x, -half.y, 0);
            // Dibujar las cuatro aristas
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}