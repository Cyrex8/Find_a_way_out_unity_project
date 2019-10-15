using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuControls : MonoBehaviour
{
    [Header("Загружаемая сцена")]
    public int sceneID;
    [Header("Остальные объекты")]
    public Image loadingImg;
    public Text loadingText;

    private int Resume;
  
        void Start()
    {
        Resume = PlayerPrefs.GetInt("LostLevel");
    }
    public void PlayPressed()
    {
        SceneManager.LoadScene("SoloLvl1");
    }
    public void PlayNetworkPressed()
    {
        SceneManager.LoadScene("DuoLvl1");
    }
    IEnumerator AsyncLoadResume()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Resume + 1);
        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            loadingImg.fillAmount = progress;
            loadingText.text = string.Format("{0:0}%", progress * 100);
            yield return null;
        }

    }

    public void ResumePressed()
    {
        StartCoroutine(AsyncLoadResume());
    }

    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }
}
