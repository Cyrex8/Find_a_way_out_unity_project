using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public float timer;
    public bool ispause;
  
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject player;
    void Update()
    {
        Time.timeScale = timer;
        if (Input.GetKeyDown(KeyCode.Escape) && ispause == false)
        {
            ispause = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && ispause == true)
        {
            ispause = false;
        }
        if (ispause == true)
        {
            timer = 0;
            pauseMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else if (ispause == false)
        {
            timer = 1f;
            pauseMenu.SetActive(false);
            Cursor.visible = false;

        }
    }
   
}
