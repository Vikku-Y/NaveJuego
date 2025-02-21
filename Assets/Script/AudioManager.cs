using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Instance.StartCoroutine(Instance.RisePitch(1));

            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", 0.8f)) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 0.5f)) * 20);
        audioMixer.SetFloat("FXVolume", Mathf.Log10(PlayerPrefs.GetFloat("FXVolume", 0.5f)) * 20);

        gameObject.GetComponentInChildren<AudioSource>().playOnAwake = false;
    }

    public void changeMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void changeMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void changeFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("FXVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("FXVolume", sliderValue);
    }

    public IEnumerator LowerPitch(float value)
    {
        audioMixer.GetFloat("MusicPitch", out float pitch);
        while (pitch > value)
        {
            pitch -= 0.015f;
            audioMixer.SetFloat("MusicPitch", pitch);
            yield return new WaitForSecondsRealtime(0.05f);
            audioMixer.GetFloat("MusicPitch", out pitch);
        }

        if (pitch < value)
        {
            audioMixer.SetFloat("MusicPitch", value);
        }
    }

    public IEnumerator RisePitch(float value)
    {
        audioMixer.GetFloat("MusicPitch", out float pitch);

        while (pitch < value)
        {
            pitch += 0.05f;
            audioMixer.SetFloat("MusicPitch", pitch);
            yield return new WaitForSecondsRealtime(0.10f);
            audioMixer.GetFloat("MusicPitch", out pitch);
        }

        if (pitch > value)
        {
            audioMixer.SetFloat("MusicPitch", value);
        }
    }
}
