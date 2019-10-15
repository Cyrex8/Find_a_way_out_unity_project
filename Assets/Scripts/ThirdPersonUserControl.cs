using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character;  // —сылка на ThirdPersonCharacter на объекте
        private Transform m_Cam;                  // —сылка на основную камеру
        private Vector3 m_CamForward;             // “екущее направление камеры
        private Vector3 m_Move;
        private bool m_Jump;                      

        
        private void Start()
        {
            // получить положение основной камеры

            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
               
            }

            // получаем компонент персонажа
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        // Fixed update вызываетс€ синхронно с физикой
        private void FixedUpdate()
        {

            // считать входные данные
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // вычисл€ем направление движени€ дл€ перехода к персонажу
            if (m_Cam != null)
            {

                // вычисл€ем относительное направление камеры дл€ перемещени€:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {

                // мы используем относительные мировые направлени€ в случае отсутстви€ основной камеры
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT

            // множитель скорости ходьбы
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // передать все параметры в скрипт управлени€ персонажем
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
