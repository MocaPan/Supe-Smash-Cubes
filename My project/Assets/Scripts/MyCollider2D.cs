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
        private static MyCollider2D[] allColliders = new MyCollider2D[10];  // tamaño inicial
        private static int colliderCount = 0;

        // Métodos de registro llamados automáticamente al habilitar/deshabilitar un collider
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
                // Aumentar el tamaño del array manualmente (doblar su capacidad)
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
                    // Desplaza el último elemento a la posición eliminada para evitar huecos
                    allColliders[i] = allColliders[colliderCount - 1];
                    allColliders[colliderCount - 1] = null;
                    colliderCount--;
                    break;
                }
            }
        }

        // Método abstracto que implementarán las subclases para dibujar la forma con Gizmos
        protected abstract void OnDrawGizmos();

        // --- Métodos estáticos de utilidad para cálculos de colisión básicos ---
        public static bool CheckCircleCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB)
        {
            float distSq = (centerB - centerA).sqrMagnitude;
            float radSum = radiusA + radiusB;
            return distSq <= radSum * radSum;
        }

        public static bool CheckRectRect(Vector2 centerA, Vector2 halfSizeA, Vector2 centerB, Vector2 halfSizeB)
        {
            // Comprueba superposición de dos AABB (rectángulos axis-aligned) usando medio anchura/altura
            return Math.Abs(centerA.x - centerB.x) <= (halfSizeA.x + halfSizeB.x) &&
                   Math.Abs(centerA.y - centerB.y) <= (halfSizeA.y + halfSizeB.y);
        }

        public static bool CheckCircleRect(Vector2 circleCenter, float radius, Vector2 rectCenter, Vector2 halfSize)
        {
            // Encuentra el punto más cercano del rectángulo al centro del círculo
            float dx = circleCenter.x - Mathf.Clamp(circleCenter.x, rectCenter.x - halfSize.x, rectCenter.x + halfSize.x);
            float dy = circleCenter.y - Mathf.Clamp(circleCenter.y, rectCenter.y - halfSize.y, rectCenter.y + halfSize.y);
            // Si la distancia desde el centro del círculo a ese punto es menor o igual al radio, hay colisión
            return (dx * dx + dy * dy) <= radius * radius;
        }

        // (Podríamos agregar más métodos de utilidad si es necesario, por ejemplo para colisión cápsula-otras formas)

        // Propiedad para acceso al registro global desde otras partes (por ejemplo, MyRigidbody2D)
        public static MyCollider2D[] AllColliders { get { return allColliders; } }
        public static int ColliderCount { get { return colliderCount; } }
    }
}
