using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LoadPlayerPrefs : MonoBehaviour
{
    [Header("~~~~General Settings~~~~`")]
    [SerializeField] private bool _CanUse = false;
    [SerializeField] private MainMenuControler menuController;

    [Header("~~~~~~Audio settings~~~~~")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("~~~Sensitivity Settings~~")]
    [SerializeField] private TMP_Text controllerSensivityTextValue = null;
    [SerializeField] private Slider conrollerSensSlider = null;

    [Header("~~~~~Quality Settings~~~~")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("~~~Brightness Settings~~~")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTexTValue = null;

    [Header("~~~~Invert Y Settings~~~~")]
    [SerializeField] private Toggle invertYtoggle = null;

    [Header("~~~FullScrenn Settings~~~")]
    [SerializeField] private Toggle fullScreenToggel;


    // Start is called before the first frame update
    void Awake()
    {
        if (_CanUse)
        {
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                float _LocalVolume = PlayerPrefs.GetFloat("MasterVolume");
                volumeTextValue.text = _LocalVolume.ToString("0.0");
                volumeSlider.value = _LocalVolume;
                AudioListener.volume = _LocalVolume;
            }
            else { menuController.Resetbutton("Sound");}

            if (PlayerPrefs.HasKey("MasterQualityLevel"))
            {
                int _LocalQuality = PlayerPrefs.GetInt("MasterQualityLevel");
                qualityDropdown.value = _LocalQuality;
                QualitySettings.SetQualityLevel(_LocalQuality);
            }

            if (PlayerPrefs.HasKey("MasterBrightness"))
            {
                float _LocalBrightness = PlayerPrefs.GetFloat("MasterBrightness");
                brightnessTexTValue.text = _LocalBrightness.ToString("0.0");
                brightnessSlider.value = _LocalBrightness;

            }

            if (PlayerPrefs.HasKey("MasterSen"))
            {
                float _LocalSensitity = PlayerPrefs.GetFloat("MasterSen");
                controllerSensivityTextValue.text = _LocalSensitity.ToString("0.0");
                conrollerSensSlider.value = _LocalSensitity;               
            }

            if (PlayerPrefs.HasKey("MasterInvertY"))
            {
                int _LocalInvert = PlayerPrefs.GetInt("MasterInvertY");
                if (_LocalInvert == 1)
                {
                    invertYtoggle.isOn = true;
                }
                else
                {
                    invertYtoggle.isOn = false;
                }
            }           

            if (PlayerPrefs.HasKey("MasterFullScreen"))
            {
                int _LocalFullScreen = PlayerPrefs.GetInt("MasterFullScreen");
                if (_LocalFullScreen == 1)
                {
                    Screen.fullScreen = true; 
                    fullScreenToggel.isOn= true;
                }
                else
                {
                    Screen.fullScreen = false;
                    fullScreenToggel.isOn = false;
                }
            }           
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
