using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deth : MonoBehaviour
{
    public GameObject Obj;
    public string NameAnimation;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider col)
    {
            Obj.GetComponent<Animation>().Play(NameAnimation);
        Debug.Log("DADADA");
     
    }
}
