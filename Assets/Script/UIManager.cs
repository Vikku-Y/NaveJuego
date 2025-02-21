using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Image hpBar;
    public Image energyBar;

    public Canvas gameUI;
    public Canvas gameOverScreen;
    public Canvas pauseMenu;
    public Canvas victoryScreen;

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider fxVolume;

    private bool gameOver = false;

    public GameObject pointer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        hpBar.type = Image.Type.Filled;
        energyBar.type = Image.Type.Filled;

        hpBar.fillMethod = Image.FillMethod.Horizontal;
        energyBar.fillMethod = Image.FillMethod.Horizontal;
        AudioManager.Instance.audioMixer.SetFloat("MusicLowpass", 22000);
        UpdateSliders();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverScreen.gameObject.activeSelf) {
            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);

            if (pauseMenu.gameObject.activeSelf) {
                pointer.SetActive(false);
                Cursor.visible = true;
                AudioManager.Instance.audioMixer.SetFloat("MusicLowpass", 1900);

                Time.timeScale = 0.00001f;
            } else
            {
                pointer.SetActive(true);
                Cursor.visible = false;
                AudioManager.Instance.audioMixer.SetFloat("MusicLowpass", 22000);

                Time.timeScale = 1f;
            }
        }

        if (GameStateManager.Instance.defeated && !gameOver) {
            gameOver = true;

            Time.timeScale = 0.5f;
            AudioManager.Instance.audioMixer.SetFloat("MusicLowpass", 1900);
            StartCoroutine(AudioManager.Instance.LowerPitch(0.5f));

            pointer.SetActive(false);
            Cursor.visible = true;

            gameUI.gameObject.SetActive(false);

            gameOverScreen.gameObject.SetActive(true);

            if (pauseMenu.gameObject.activeSelf) { 
                pauseMenu.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateSliders()
    {
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        fxVolume.value = PlayerPrefs.GetFloat("FXVolume", 0.5f);
    }
}
