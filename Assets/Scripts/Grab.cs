using UnityEngine;
using System.Collections;
public class Grab : MonoBehaviour
{

    public float СкоростьМагнита = 10; //Скорость Магнита	 
    public float СкоростьОтталкивания = 20; //Скорость Отталкивания	 
    public float ДлинаЛуча = 10; //Длина луча	 
    public RaycastHit Попадание; //Луч который...	 
    private bool ВзятьБросить = false; //Отключили	 
    private bool Толкнуть = false; //Отключили	 
    public Transform ПозицияМагнита; //Позиция магнита	 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //Условие выполняется пока зажата кнопка клавиатуры	 
        {
            Physics.Raycast(transform.position, transform.forward, out Попадание, ДлинаЛуча);
            if (Попадание.rigidbody)
            {
                ВзятьБросить = true; //Включили	 
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ВзятьБросить)
            {
                ВзятьБросить = false; //Отключили	 
                Толкнуть = true; //Включили	 
            }
        }
        if (ВзятьБросить)
        {
            if (Попадание.rigidbody)
            {

                Попадание.rigidbody.freezeRotation = true;
                Попадание.rigidbody.velocity = (ПозицияМагнита.position - (Попадание.transform.position + Попадание.rigidbody.centerOfMass)) * СкоростьМагнита;
           
            }
        }
        if (Толкнуть)
        {
            if (Попадание.rigidbody)
            {
                Попадание.rigidbody.freezeRotation = false;
                Попадание.rigidbody.velocity = transform.forward * СкоростьОтталкивания;
                Толкнуть = false; //Отключили	 
            }
        }
    }
}