using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Cameras
{
    public class FreeLookCam : PivotBasedCameraRig
    {
        // 	Camera Rig
        // 		Pivot
        // 			Camera

        [SerializeField] private float m_MoveSpeed = 1f;                      // Как быстро будет двигаться буровая установка, чтобы не отставать от позиции цели.
        [Range(0f, 10f)] [SerializeField] private float m_TurnSpeed = 1.5f;   // Как быстро будет вращаться установка из пользовательского ввода.
        [SerializeField] private float m_TurnSmoothing = 0.1f;                // Как много сглаживания применить к повороту, чтобы уменьшить рывки поворота мыши.
        [SerializeField] private float m_TiltMax = 75f;                       // Максимальное значение поворота оси x.
        [SerializeField] private float m_TiltMin = 45f;                       // Минимальное значение поворота оси x.
        [SerializeField] private bool m_LockCursor = false;                   // Должен ли курсор быть скрыт и заблокирован.
        [SerializeField] private bool m_VerticalAutoReturn = false;           // установить, следует ли автоматически возвращать вертикальную ось.

        private float m_LookAngle;                    // Поворот y
        private float m_TiltAngle;                    // Поворот x
        private const float k_LookDistance = 100f;    // Как далеко перед точкой поворота находится цель взгляда персонажа.
        private Vector3 m_PivotEulers;
		private Quaternion m_PivotTargetRot;
		private Quaternion m_TransformTargetRot;
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //anglePitch = 0;

        }
        protected override void Awake()
        {
            base.Awake();
            // Блокировка или разблокировка курсора.
            Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !m_LockCursor;
			m_PivotEulers = m_Pivot.rotation.eulerAngles;

	        m_PivotTargetRot = m_Pivot.transform.localRotation;
			m_TransformTargetRot = transform.localRotation;
        }


        protected void Update()
        {
            HandleRotationMovement();
            if (m_LockCursor && Input.GetMouseButtonUp(0))
            {
                Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !m_LockCursor;
            }
        }


        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        protected override void FollowTarget(float deltaTime)
        {
            if (m_Target == null) return;
            // Перемещение.
            transform.position = Vector3.Lerp(transform.position, m_Target.position, deltaTime*m_MoveSpeed);
        }


        private void HandleRotationMovement()
        {
			if(Time.timeScale < float.Epsilon)
			return;

            // Считываем пользовательский ввод
            var x = CrossPlatformInputManager.GetAxis("Mouse X");
            var y = CrossPlatformInputManager.GetAxis("Mouse Y");

            // Регулируем угол обзора на величину, пропорциональную скорости поворота и горизонтальному вводу.
            m_LookAngle += x*m_TurnSpeed;

            // Вращаем rig (корневой объект) только вокруг оси Y
            m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

            if (m_VerticalAutoReturn)
            {
                // Для наклона нам нужно вести себя по-разному в зависимости от того, используем мы мышь или сенсорный ввод
                // на мобильном устройстве вертикальный ввод напрямую сопоставляется со значением наклона, поэтому он автоматически возвращается обратно
                m_TiltAngle = y > 0 ? Mathf.Lerp(0, -m_TiltMin, y) : Mathf.Lerp(0, m_TiltMax, -y);
            }
            else
            {

                // на устройствах с мышью мы корректируем текущий угол на основе ввода мыши Y и скорости поворота
                m_TiltAngle -= y*m_TurnSpeed;
                // НЕовое значение находится в диапазоне наклона
                m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);
            }

            // Наклон вокруг X применяется к оси вращения (дочернему объекту этого объекта)
            m_PivotTargetRot = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y , m_PivotEulers.z);

			if (m_TurnSmoothing > 0)
			{
				m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);
				transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
			}
			else
			{
				m_Pivot.localRotation = m_PivotTargetRot;
				transform.localRotation = m_TransformTargetRot;
			}
        }
    }
}
