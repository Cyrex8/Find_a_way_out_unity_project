using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelDone : MonoBehaviour
{
    public int currentLevel;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            PlayerPrefs.SetInt("LostLevel", currentLevel); //ЗАПИСЫВАЕМ НОМЕР УРОВНЯ
        }
    }

}