using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public bool defeated { get; private set; }

    public static GameStateManager Instance { get; private set; }

    private void Awake()
    {
        defeated = false;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Defeat()
    {
        defeated = true;
    }

    public void RestartGame()
    {   
        defeated = false;
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
        UIManager.Instance.gameOverScreen.gameObject.SetActive(false);
        defeated = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
