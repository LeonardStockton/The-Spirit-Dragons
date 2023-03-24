using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeControl : MonoBehaviour
{
    public static VolumeControl instance;

    [SerializeField] string _VolumeParameter = "Volume Source";
    [SerializeField] AudioMixer _Mixer;
    [SerializeField] Slider _Slider;
    [SerializeField] float _Multiplyer = 30f;
    [SerializeField] Toggle _Mute;
    [SerializeField] bool _DisableToggleEvent;

    // Start is called before the first frame update
    void Awake()
    {
        _Slider.onValueChanged.AddListener(VolumeController);
        _Mute.onValueChanged.AddListener(HandleToggleValueChange);
    }

    public void HandleToggleValueChange(bool _IsSoundOn)
    {
        if (_DisableToggleEvent) { return; }
        if (_IsSoundOn) { _Slider.value = _Slider.maxValue; }
        else { _Slider.value = _Slider.minValue; }
    }

    public void OnDisable()
    {
        PlayerPrefs.SetFloat(_VolumeParameter, _Slider.value);
    }
    public void Start()
    {
        _Slider.value = PlayerPrefs.GetFloat(_VolumeParameter, _Slider.value);
    }
    void Update()
    {

    }

    public void VolumeController(float value)
    {
        _Mixer.SetFloat(_VolumeParameter, Mathf.Log10(value) * _Multiplyer);
        _DisableToggleEvent = true;
        _Mute.isOn = _Slider.value > _Slider.minValue;
        _DisableToggleEvent= false;
        if(_Slider.value<=0) { _Multiplyer= 0; }
    }



    // Update is called once per frame
}
