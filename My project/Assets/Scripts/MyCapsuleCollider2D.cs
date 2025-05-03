using UnityEngine;

namespace CustomPhysics2D
{

    // Colisionador de c�psula 2D (vertical). Si height <= 2*radius act�a como colisionador circular.
    public class MyCapsuleCollider2D : MyCollider2D
    {
        public float radius = 0.5f;  // Radio de los extremos de la c�psula (y del c�rculo si aplica)
        public float height = 1.0f;  // Altura total de la c�psula (distancia de extremo a extremo)

        // Dibuja la c�psula (o c�rculo) en la vista de Escena usando Gizmos
        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            // Si la altura es peque�a, dibujamos un c�rculo
            if (height <= 2 * radius)
            {
                Gizmos.DrawWireSphere(transform.position, radius);
            }
            else
            {
                // C�psula: dibujar dos semic�rculos (arriba y abajo) y un rect�ngulo central
                float halfHeight = height / 2f;
                float flatHeight = height - 2 * radius;  // altura de la secci�n central recta
                Vector3 center = transform.position;
                // Centros de las semiesferas superior e inferior
                Vector3 topCenter = center + new Vector3(0, halfHeight - radius, 0);
                Vector3 bottomCenter = center + new Vector3(0, -halfHeight + radius, 0);
                // Dibujar c�rculos (se ver�n como esferas al no tener Gizmo espec�fico 2D)
                Gizmos.DrawWireSphere(topCenter, radius);
                Gizmos.DrawWireSphere(bottomCenter, radius);
                // Dibujar los lados de la secci�n central
                Vector3 leftTop = topCenter + new Vector3(-radius, 0, 0);
                Vector3 leftBottom = bottomCenter + new Vector3(-radius, 0, 0);
                Vector3 rightTop = topCenter + new Vector3(radius, 0, 0);
                Vector3 rightBottom = bottomCenter + new Vector3(radius, 0, 0);
                Gizmos.DrawLine(leftTop, leftBottom);
                Gizmos.DrawLine(rightTop, rightBottom);
            }
        }
    }
}