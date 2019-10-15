using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabUI : MonoBehaviour
{
    public float speed = 10;
    public bool canHold = true;
    public Transform guide;
    public GameObject player;

    void OnCollisionEnter(Collision col)
    {
        if (player.tag == "Player") { }
        
            Debug.Log("YES");
            if (Input.GetKeyDown(KeyCode.E))
            {

               gameObject.transform.position = guide.position;

            }//update
        
        //We can use trigger or Collision

    }
}