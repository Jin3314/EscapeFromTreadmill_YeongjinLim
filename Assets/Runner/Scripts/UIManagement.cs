using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagement : MonoBehaviour
{
    [SerializeField] Text startPauseText;
    bool pauseActive = false;

    public GameObject PausePanel;

    void Start()
    {
      
    }
    public void pauseBtn()
    {
        if (pauseActive)
        {
            PausePanel.SetActive(false);
            Time.timeScale = 1;
            pauseActive = false;
        }
        else
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0;
            pauseActive = true;
        }

        startPauseText.text = pauseActive ? "START" : "PAUSE";
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
    }

    public void Instruction()
    {
        SceneManager.LoadScene("Instruction");
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}