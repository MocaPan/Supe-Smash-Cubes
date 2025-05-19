using UnityEngine;
using System;  // (Usado solo para Array.Copy, evitamos List<T>)

namespace CustomPhysics2D
{
    // Clase base para todos los colisionadores 2D personalizados
    public abstract class MyCollider2D : MonoBehaviour
    {
        // Determina si el collider es de tipo trigger (no bloquea movimiento, solo eventos)
        public bool isTrigger = false;

        // Registro global de todos los colliders activos en la escena (evitando List<T>)
        private static MyCollider2D[] allColliders = new MyCollider2D[10];  // tama�o inicial
        private static int colliderCount = 0;

        // M�todos de registro llamados autom�ticamente al habilitar/deshabilitar un collider
        protected virtual void OnEnable()
        {
            RegisterCollider(this);
        }
        protected virtual void OnDisable()
        {
            UnregisterCollider(this);
        }

        // Agrega un collider al registro global, expandiendo el array si es necesario
        private static void RegisterCollider(MyCollider2D col)
        {
            if (colliderCount >= allColliders.Length)
            {
                // Aumentar el tama�o del array manualmente (doblar su capacidad)
                MyCollider2D[] newArray = new MyCollider2D[allColliders.Length * 2];
                Array.Copy(allColliders, newArray, allColliders.Length);
                allColliders = newArray;
            }
            allColliders[colliderCount++] = col;
        }

        // Remueve un collider del registro global, manteniendo compacto el array
        private static void UnregisterCollider(MyCollider2D col)
        {
            for (int i = 0; i < colliderCount; i++)
            {
                if (allColliders[i] == col)
                {
                    // Desplaza el �ltimo elemento a la posici�n eliminada para evitar huecos
                    allColliders[i] = allColliders[colliderCount - 1];
                    allColliders[colliderCount - 1] = null;
                    colliderCount--;
                    break;
                }
            }
        }

        // M�todo abstracto que implementar�n las subclases para dibujar la forma con Gizmos
        protected abstract void OnDrawGizmos();

        // --- M�todos est�ticos de utilidad para c�lculos de colisi�n b�sicos ---
        public static bool CheckCircleCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB)
        {
            float distSq = (centerB - centerA).sqrMagnitude;
            float radSum = radiusA + radiusB;
            return distSq <= radSum * radSum;
        }

        public static bool CheckRectRect(Vector2 centerA, Vector2 halfSizeA, Vector2 centerB, Vector2 halfSizeB)
        {
            // Comprueba superposici�n de dos AABB (rect�ngulos axis-aligned) usando medio anchura/altura
            return Math.Abs(centerA.x - centerB.x) <= (halfSizeA.x + halfSizeB.x) &&
                   Math.Abs(centerA.y - centerB.y) <= (halfSizeA.y + halfSizeB.y);
        }

        public static bool CheckCircleRect(Vector2 circleCenter, float radius, Vector2 rectCenter, Vector2 halfSize)
        {
            // Encuentra el punto m�s cercano del rect�ngulo al centro del c�rculo
            float dx = circleCenter.x - Mathf.Clamp(circleCenter.x, rectCenter.x - halfSize.x, rectCenter.x + halfSize.x);
            float dy = circleCenter.y - Mathf.Clamp(circleCenter.y, rectCenter.y - halfSize.y, rectCenter.y + halfSize.y);
            // Si la distancia desde el centro del c�rculo a ese punto es menor o igual al radio, hay colisi�n
            return (dx * dx + dy * dy) <= radius * radius;
        }

        // (Podr�amos agregar m�s m�todos de utilidad si es necesario, por ejemplo para colisi�n c�psula-otras formas)

        // Propiedad para acceso al registro global desde otras partes (por ejemplo, MyRigidbody2D)
        public static MyCollider2D[] AllColliders { get { return allColliders; } }
        public static int ColliderCount { get { return colliderCount; } }
    }
}
