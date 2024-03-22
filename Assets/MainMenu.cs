using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject settingButtons;
    [SerializeField] private TMP_Dropdown graphicsDropDown;
    [SerializeField] private Slider masterVol, musicVol, sfxVol;
    [SerializeField] private AudioMixer audioMixer;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenSettings()
    {
        mainMenuButtons.SetActive(false);
        settingButtons.SetActive(true);
    }

    public void SetGraphics()
    {
        QualitySettings.SetQualityLevel(graphicsDropDown.value);
    }

    public void CloseSettings()
    {
        mainMenuButtons.SetActive(true);
        settingButtons.SetActive(false);
    }

    public void ChangeMasterVolume()
    {
       audioMixer.SetFloat("Master", masterVol.value);
    }
    public void ChangeSFXVolume()
    {
        audioMixer.SetFloat("SFX", sfxVol.value);
    }
    public void ChangeMusicVolume()
    {
        audioMixer.SetFloat("Music", musicVol.value);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
	Application.Quit(); 
#endif
    }
}
