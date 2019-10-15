using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    //GameObject mainCamera;
    bool carrying;
    GameObject carriedObject;
    Camera cam;
    public float distances;
    public float smooth;

    // Use this for initialization
    void Start()
    {
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check for user input ('T' key here) and make sure the object is being carried
        if (Input.GetKeyDown(KeyCode.T) && carrying)
        {
            carrying = !carrying;
            ThrowBall();
        }
        if (carrying)
        {
            carry(carriedObject);
        }
        else
        {
            pickup();
        }

    }

    private void pickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;
            Ray ray = cam.ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Pickupable p = hit.collider.GetComponent<Pickupable>();
                if (p != null)
                {
                    carrying = true;
                    carriedObject = p.gameObject;
                }
            }
        }
    }

    void carry(GameObject o)
    {
        o.GetComponent<Rigidbody>().isKinematic = true;
        o.transform.position = cam.transform.position + new Vector3(0f, 0f, 10f);
    }

    void ThrowBall()
    {
        carriedObject.GetComponent<Rigidbody>().isKinematic = false;
        carriedObject.GetComponent<Rigidbody>().AddForce(0f, 0f, 100f);
    }
}