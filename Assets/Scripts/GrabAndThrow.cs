using UnityEngine;
using System.Collections;
public class GrabAndThrow : MonoBehaviour
{
    public float grabPower = 10f;
    public float throwPower = 9f;
    public float rayDistance = 3.0f;
    public RaycastHit hit;
    private bool Grab = false;
    private bool Throw = false;
    public Transform offset;

    // Use this for initialization 
    // Update is called once per frame 
    void Update () {
        if (Input.GetMouseButtonDown (1))
        { Physics.Raycast (transform.position, transform.forward,out hit, rayDistance);
            if (hit.rigidbody) { Grab = true;
            }
        }
        if (Input.GetMouseButtonDown (0)) {
            if (Grab) { Grab = false; Throw = true;
            }
        }
        if (Grab) { if (hit.rigidbody)
            { hit.rigidbody.velocity = (offset.position - (hit.transform.position + hit.rigidbody.centerOfMass)) * grabPower;
            }
        }
        if (Throw) { if (hit.rigidbody) { hit.rigidbody.velocity = transform.forward * throwPower; Throw = false;
            }
        }
    }
}