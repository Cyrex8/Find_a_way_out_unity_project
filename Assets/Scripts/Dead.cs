using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{

    public GameObject player;
    public GameObject playerRagdoll;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerRagdoll.transform.position = player.transform.position;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag=="Player")
        {
            player.SetActive(false);
            playerRagdoll.SetActive(true);

        }


    }

}
