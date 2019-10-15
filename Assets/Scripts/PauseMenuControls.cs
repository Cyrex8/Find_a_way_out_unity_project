using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenuControls : MonoBehaviour
{
    
   
    public void ResumePressed(GameObject obj)
    {
        obj.GetComponent<Pause>().ispause = false; 
    }
    public void SavePressed()
    {
        SceneManager.LoadScene("SoloLvl1");
    }
    public void ExitPressed()
    {
        Application.Quit();
    }
    public void MainMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
