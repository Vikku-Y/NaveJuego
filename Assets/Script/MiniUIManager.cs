using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniUIManager : MonoBehaviour
{
    public Canvas UI;

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider fxVolume;

    void Start()
    {
        UpdateSliders();
    }

    public void UpdateSliders()
    {
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        fxVolume.value = PlayerPrefs.GetFloat("FXVolume", 0.5f);
    }
}
