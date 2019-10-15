using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayerPrefs : MonoBehaviour
{
    int _NumLev; //Переменная номера уровня
    int _is; //Переменная для проверки, был ли уровень пройден


    void Start()
    {
        _is = PlayerPrefs.GetInt("is");

        if (_is == 0)
            _NumLev++;
        _is++;
        PlayerPrefs.SetInt("is", _is);
    }

    //Метод который должен вызываться в конце уровня
    void EndLevel()
    {
        _is--;
        PlayerPrefs.SetInt("is", _is);
    }
}
