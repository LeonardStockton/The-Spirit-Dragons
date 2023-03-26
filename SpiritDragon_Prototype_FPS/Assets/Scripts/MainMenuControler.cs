using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class MainMenuControler : MonoBehaviour
{
    public static MainMenuControler instance;

    [Header("~~~~~~~Audio Mixer~~~~~~~")]
    [SerializeField] public AudioMixer _Mixer;
    [SerializeField] public float _Multiplyer = 30f;

    [Header("~~~~~~Audio settings~~~~~")]
    [SerializeField] public TMP_Text masterVolumeTextValue = null;
    [SerializeField] public Slider masterVolumeSlider = null;
    [SerializeField] public TMP_Text MusicVolTextValue = null;
    [SerializeField] public Slider MusicVolSlider = null;
    [SerializeField] public TMP_Text sFXVolumeTextValue = null;
    [SerializeField] public Slider sFXVolumeSlider = null;
    [SerializeField] public float defualtVolumeValue = .8f;

    [Header("~~~~Gameplay Settings~~~~")]
    [SerializeField] public TMP_Text controllerSensivityTextValue = null;
    [SerializeField] public Slider conrollerSensSlider = null;
    [SerializeField] public int defualSensivityValue = 4;
    public int mainControllerSens = 4;

    [Header("~~~~~~~~~~Toggels~~~~~~~~")]
    [SerializeField] public Toggle invertYtoggle = null;
    [SerializeField] public Toggle fullScreenToggel;

    [Header("~~~~~~~~~~Graphics~~~~~~~~")]
    [SerializeField] public Slider brightnessSlider;
    [SerializeField] public TMP_Text brightnessTexTValue = null;
    [SerializeField] public float defualtBrightness = 1.0f;
    public PostProcessProfile brightness;
    public PostProcessLayer layer;
    AutoExposure exposure;

    [Header("~~~~~~level to load~~~~~~")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSaveFile = null;
    public float [] enemyDifficulty;
    public TMP_Dropdown gameDifficulty;

    [Header("Resolutions Settings")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] _Resolutions;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    public int _QualityLevel;
    public float _BrightnessLevel;
    public bool _IsFullScreen;

    [Header("~~confermation~~")]
    [SerializeField] private GameObject confermationPrompt = null;

    public bool isDifficult = false;
    public float _Volume;

   

    // Start is called before the first frame update
    void Start()
    {
        brightness.TryGetSettings(out exposure);
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

    public void OnDisable()
    {
        //PlayerPrefs.SetFloat()
    }
    public void GameDif()
    {
        enemyDifficulty = new float[4]; 
        if (gameDifficulty.value == 0 ) 
        {
            isDifficult = true;
            enemyDifficulty[1] = 2f;
        }
        else if (gameDifficulty.value == 1) 
        { 
            enemyDifficulty[1] =  1f; 
        }
        else if(gameDifficulty.value == 2) 
        { 
            enemyDifficulty[2] =  0.5f;
        }
        else
        { 
            enemyDifficulty[3] =  0.25f;
        }
    }
    public void GameDifApply()
    {
        for (int i = 0; i < enemyDifficulty.Length; i++)
        {
            if (enemyDifficulty[i] == 0)
            {
                enemyDifficulty[i] = gameDifficulty.value;
                PlayerPrefs.SetInt("Game Difficulty", i);
            }
            if (enemyDifficulty[i] == 1)
            {
                enemyDifficulty[i] = gameDifficulty.value;
                PlayerPrefs.SetInt("Game Difficulty", i);
            }
            if (enemyDifficulty[i] == 2) 
            {
                enemyDifficulty[i] = gameDifficulty.value;
                PlayerPrefs.SetInt("Game Difficulty", i);
            }
            if (enemyDifficulty[i] == 3) 
            {
                enemyDifficulty[i] = gameDifficulty.value;
                PlayerPrefs.SetInt("Game Difficulty", i);
            }
        }
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

    public void SetVolume(float volumeLevel )
    {
        masterVolumeTextValue.text = volumeLevel.ToString("0.0");
        MusicVolTextValue.text = volumeLevel.ToString("0.0");
        sFXVolumeTextValue.text = volumeLevel.ToString("0.0");

    }

    public void VolumeApply()
    {
      
        PlayerPrefs.SetFloat("MasterVolume", AudioListener.volume);
        PlayerPrefs.SetFloat("MusicVolume", AudioListener.volume);
        PlayerPrefs.SetFloat("SFX Volume", AudioListener.volume);
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
        { PlayerPrefs.SetInt("MasterInvertY", 1); }
        //not inverted 
        else
        { PlayerPrefs.SetInt("MasterInvertY", 0); }

        PlayerPrefs.SetFloat("MasterSen", mainControllerSens);
        StartCoroutine(ConfermationBox());
    }

    public void SetBtightness(float bright)
    {
        _BrightnessLevel = bright;
        brightnessTexTValue.text = bright.ToString("0.0");
        if (bright != 0)
        {
            exposure.keyValue.value = bright;
        }
        else
        {
            exposure.keyValue.value = 0.05f;
        }
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
        PlayerPrefs.SetFloat("MasterBrightness", _BrightnessLevel);
        //chage the Brightness with post processing.

        PlayerPrefs.SetInt("MasterQualityLevel", _QualityLevel);
        QualitySettings.SetQualityLevel(_QualityLevel);

        PlayerPrefs.SetInt("MasterFullScreen", (_IsFullScreen ? 1 : 0));
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

            fullScreenToggel.isOn = false;
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
