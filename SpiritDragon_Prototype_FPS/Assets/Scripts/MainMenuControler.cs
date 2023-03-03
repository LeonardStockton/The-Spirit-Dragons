using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class MainMenuControler : MonoBehaviour
{
    [Header("~~~~~~Audio settings~~~~~")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defualtVolume = 1.0f;

    [Header("~~~~Gameplay Settings~~~~")]
    [SerializeField] private TMP_Text controllerSensivityTextValue = null;
    [SerializeField] private Slider conrollerSensSlider = null;
    [SerializeField] private int defualSensivityValue = 4;
    public int mainControllerSens = 4;

    [Header("~~~~~~~~~~Toggels~~~~~~~~")]
    [SerializeField] private Toggle invertYtoggle = null;
    [SerializeField] private Toggle fullScreenToggel;
    [Header("~~~~~~~~~~Graphics~~~~~~~~")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTexTValue = null;
    [SerializeField] private float defualtBrightness = 1.0f;


    [Header("~~~~~~level to load~~~~~~")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSaveFile = null;

    [Header("Resolutions Settings")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] _Resolutions;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    private int _QualityLevel;
    private float _BrightnessLevel;
    private bool _IsFullScreen;

    [Header("~~confermation~~")]
    [SerializeField] private GameObject confermationPrompt = null;


    // Start is called before the first frame update
    void Start()
    {
        _Resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currenResolutionIndex = 0;
        for (int i = 0; i < _Resolutions.Length; i++)
        {
            string option = _Resolutions[i].width + " x " + _Resolutions[i].height;
            options.Add(option);
            if (_Resolutions[i].width == Screen.width && _Resolutions[i].height == Screen.height)
            {
                currenResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currenResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    // Update is called once per frame
    void Update()
    {

    }
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

    public void SetControllerSenstivity(float sensitivity)
    {
        mainControllerSens = Mathf.RoundToInt(sensitivity);
        controllerSensivityTextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        //invert y
        if (invertYtoggle.isOn)
        { PlayerPrefs.SetInt("masterInvertY", 1); }
        //not inverted 
        else
        { PlayerPrefs.SetInt("masterInvertY", 0); }

        PlayerPrefs.SetFloat("masterSen", mainControllerSens);
        StartCoroutine(ConfermationBox());
    }

    public void SetBtightness(float bright)
    {
        _BrightnessLevel = bright;
        brightnessTexTValue.text = bright.ToString("0.0");
    }

    public void ToggelFullScreen(bool isFullScreen)
    {
        _IsFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _QualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("_MasterBrightness", _BrightnessLevel);
        //chage the Brightness with post processing.

        PlayerPrefs.SetInt("_MasterQuality Level", _QualityLevel);
        QualitySettings.SetQualityLevel(_QualityLevel);

        PlayerPrefs.SetInt("_MasterFullScreen", (_IsFullScreen ? 1 : 0));
        Screen.fullScreen = _IsFullScreen;

        StartCoroutine(ConfermationBox());
    }

    public void SetResalution(int resalutionIndex)
    {
        Resolution resolution = _Resolutions[resalutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void Resetbutton(string menuType)
    {
        if (menuType == "Sound")
        {
            AudioListener.volume = defualtVolume;
            volumeSlider.value = defualtVolume;
            volumeTextValue.text = defualtVolume.ToString("0.0");
            VolumeApply();
        }
        if (menuType == "Gameplay")
        {
            controllerSensivityTextValue.text = defualSensivityValue.ToString("0");
            conrollerSensSlider.value = defualSensivityValue;
            mainControllerSens = defualSensivityValue;
            invertYtoggle.isOn = false;
            GameplayApply();
        }
        if (menuType == "Graphics")
        {
            brightnessTexTValue.text = defualtBrightness.ToString("0");
            brightnessSlider.value = defualtBrightness;

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggel.isOn= false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = _Resolutions.Length;
            GraphicsApply();
        }
    }
    public IEnumerator ConfermationBox()
    {
        confermationPrompt.SetActive(true);
        yield return new WaitForSeconds(3);
        confermationPrompt.SetActive(false);

    }
}
