using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Cameras
{
    public class ProtectCameraFromWallClip : MonoBehaviour
    {
        public float clipMoveTime = 0.05f;              // время, необходимое для перемещения, чтобы избежать клипирования 
        public float returnTime = 0.4f;                 // время, необходимое для перемещения назад к желаемой позиции, когда оно не ограничено (обычно должно быть больше, чем clipMoveTime)
        public float sphereCastRadius = 0.1f;           // радиус сферы, используемой для проверки объекта между камерой и целью
        public bool visualiseInEditor;                  // переключатель для визуализации алгоритма через строки для raycast в редакторе
        public float closestDistance = 0.5f;            // ближайшее расстояние камеры от цели
        public bool protecting { get; private set; }    // используется для определения наличия объекта между целью и камерой
        public string dontClipTag = "Player";           // не обрезать объекты с этим тегом (полезно, чтобы не обрезать целевой объект)

        private Transform m_Cam;                   // Смещение камеры
        private Transform m_Pivot;                 // точка, вокруг которой камера поворачивается
        private float m_OriginalDist;              // исходное расстояние до камеры перед внесением каких-либо изменений
        private float m_MoveVelocity;              // скорость движения камеры
        private float m_CurrentDist;               // текущее расстояние от камеры до цели
        private Ray m_Ray;                         // луч, создаваемый между камерой и целью
        private RaycastHit[] m_Hits;               // попадание между камерой и целью
        private RayHitComparer m_RayHitComparer;   // переменная для сравнения расстояний попадания


        private void Start()
        {
            // найти камеру в иерархии объектов
            m_Cam = GetComponentInChildren<Camera>().transform;
            m_Pivot = m_Cam.parent;
            m_OriginalDist = m_Cam.localPosition.magnitude;
            m_CurrentDist = m_OriginalDist;

            // новый RayHitComparer
            m_RayHitComparer = new RayHitComparer();
        }


        private void LateUpdate()
        {
            // initially set the target distance
            float targetDist = m_OriginalDist;

            m_Ray.origin = m_Pivot.position + m_Pivot.forward*sphereCastRadius;
            m_Ray.direction = -m_Pivot.forward;

            // Пересекает ли сфера что-либо
            var cols = Physics.OverlapSphere(m_Ray.origin, sphereCastRadius);

            bool initialIntersect = false;
            bool hitSomething = false;

            // перебираем все варианты
            for (int i = 0; i < cols.Length; i++)
            {
                if ((!cols[i].isTrigger) &&
                    !(cols[i].attachedRigidbody != null && cols[i].attachedRigidbody.CompareTag(dontClipTag)))
                {
                    initialIntersect = true;
                    break;
                }
            }

            // если есть столкновение
            if (initialIntersect)
            {
                m_Ray.origin += m_Pivot.forward*sphereCastRadius;

                // сделать raycast и собрать все пересечения
                m_Hits = Physics.RaycastAll(m_Ray, m_OriginalDist - sphereCastRadius);
            }
            else
            {
                // если не было столкновений, создаем сферу, чтобы увидеть, были ли другие столкновения
                m_Hits = Physics.SphereCastAll(m_Ray, sphereCastRadius, m_OriginalDist + sphereCastRadius);
            }

            // сортируем столкновения по расстоянию
            Array.Sort(m_Hits, m_RayHitComparer);
            // устанавливаем переменную, используемую для хранения ближайшего
            float nearest = Mathf.Infinity;

            // цикл через все столкновения
            for (int i = 0; i < m_Hits.Length; i++)
            {
                //обрабатывать столкновение, только если оно ближе предыдущего
                if (m_Hits[i].distance < nearest && (!m_Hits[i].collider.isTrigger) &&
                    !(m_Hits[i].collider.attachedRigidbody != null &&
                      m_Hits[i].collider.attachedRigidbody.CompareTag(dontClipTag)))
                {
                    // изменить ближайшее столкновение на последнее
                    nearest = m_Hits[i].distance;
                    targetDist = -m_Pivot.InverseTransformPoint(m_Hits[i].point).z;
                    hitSomething = true;
                }
            }

       
            if (hitSomething)
            {
                Debug.DrawRay(m_Ray.origin, -m_Pivot.forward*(targetDist + sphereCastRadius), Color.red);
            }

            //Переместить камеру в лучшее положение
            protecting = hitSomething;
            m_CurrentDist = Mathf.SmoothDamp(m_CurrentDist, targetDist, ref m_MoveVelocity,
                                           m_CurrentDist > targetDist ? clipMoveTime : returnTime);
            m_CurrentDist = Mathf.Clamp(m_CurrentDist, closestDistance, m_OriginalDist);
            m_Cam.localPosition = -Vector3.forward*m_CurrentDist;
        }


        // comparer для проверки расстояний при попадании лучей
        public class RayHitComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return ((RaycastHit) x).distance.CompareTo(((RaycastHit) y).distance);
            }
        }
    }
}
