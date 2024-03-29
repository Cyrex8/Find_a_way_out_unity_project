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

        [SerializeField] private float m_MoveSpeed = 1f;                      // ��� ������ ����� ��������� ������� ���������, ����� �� ��������� �� ������� ����.
        [Range(0f, 10f)] [SerializeField] private float m_TurnSpeed = 1.5f;   // ��� ������ ����� ��������� ��������� �� ����������������� �����.
        [SerializeField] private float m_TurnSmoothing = 0.1f;                // ��� ����� ����������� ��������� � ��������, ����� ��������� ����� �������� ����.
        [SerializeField] private float m_TiltMax = 75f;                       // ������������ �������� �������� ��� x.
        [SerializeField] private float m_TiltMin = 45f;                       // ����������� �������� �������� ��� x.
        [SerializeField] private bool m_LockCursor = false;                   // ������ �� ������ ���� ����� � ������������.
        [SerializeField] private bool m_VerticalAutoReturn = false;           // ����������, ������� �� ������������� ���������� ������������ ���.

        private float m_LookAngle;                    // ������� y
        private float m_TiltAngle;                    // ������� x
        private const float k_LookDistance = 100f;    // ��� ������ ����� ������ �������� ��������� ���� ������� ���������.
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
            // ���������� ��� ������������� �������.
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
            // �����������.
            transform.position = Vector3.Lerp(transform.position, m_Target.position, deltaTime*m_MoveSpeed);
        }


        private void HandleRotationMovement()
        {
			if(Time.timeScale < float.Epsilon)
			return;

            // ��������� ���������������� ����
            var x = CrossPlatformInputManager.GetAxis("Mouse X");
            var y = CrossPlatformInputManager.GetAxis("Mouse Y");

            // ���������� ���� ������ �� ��������, ���������������� �������� �������� � ��������������� �����.
            m_LookAngle += x*m_TurnSpeed;

            // ������� rig (�������� ������) ������ ������ ��� Y
            m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

            if (m_VerticalAutoReturn)
            {
                // ��� ������� ��� ����� ����� ���� ��-������� � ����������� �� ����, ���������� �� ���� ��� ��������� ����
                // �� ��������� ���������� ������������ ���� �������� �������������� �� ��������� �������, ������� �� ������������� ������������ �������
                m_TiltAngle = y > 0 ? Mathf.Lerp(0, -m_TiltMin, y) : Mathf.Lerp(0, m_TiltMax, -y);
            }
            else
            {

                // �� ����������� � ����� �� ������������ ������� ���� �� ������ ����� ���� Y � �������� ��������
                m_TiltAngle -= y*m_TurnSpeed;
                // ������ �������� ��������� � ��������� �������
                m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);
            }

            // ������ ������ X ����������� � ��� �������� (��������� ������� ����� �������)
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
