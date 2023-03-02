using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuControler : MonoBehaviour
{
    [Header("~~~~~~level to load~~~~~~")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSaveFile = null;

    [Header("~~~~~~Audio settings~~~~~")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defualtVolume = 1.0f;

    [SerializeField] private GameObject confermationPrompt=null;

    public void NewGameDialog()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadGameDialog()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSaveFile.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVaolume", AudioListener.volume);
        //show prompt 
        StartCoroutine(ConfermationBox());
    }
    public IEnumerator ConfermationBox()
    {
        confermationPrompt.SetActive(true);
        yield return new WaitForSeconds(3);
        confermationPrompt.SetActive(false);

    }

    public void Resetbutton(string menuType)
    {
        if (menuType=="Sound")
        {
            AudioListener.volume = defualtVolume;
            volumeSlider.value = defualtVolume;
            volumeTextValue.text = defualtVolume.ToString("0.0");
            VolumeApply();
        }

    }
}
