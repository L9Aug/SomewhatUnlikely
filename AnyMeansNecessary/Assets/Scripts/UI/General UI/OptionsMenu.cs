using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{

    public Slider VolumeSlider;
    public Dropdown fullscreen;
    public Dropdown resolutionDropDown;
    public Dropdown Quality;
    bool isWindowed;
    Resolution[] resolutions;


    // Use this for initialization
    void Start()
    {
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionDropDown.options.Add(new Dropdown.OptionData(ResolutionToString(resolutions[i])));
            resolutionDropDown.value = i;


        }
        Quality.value = QualitySettings.GetQualityLevel();
       
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = VolumeSlider.value;

    }

    public void QualitySetting()
    {
        QualitySettings.SetQualityLevel(Quality.value);
    }
    

    public void isFullScreen()
    {
        if (fullscreen.value == 0)
        {
            Screen.fullScreen = true;
            isWindowed = false;
        }

        else
        {
            Screen.fullScreen = false;
            isWindowed = true;
        }
    }

    public string ResolutionToString(Resolution res)
    {
        return res.width + "x" + res.height;
    }

    public void ScreenRes()
    {
        if(isWindowed == false)
        {
            Screen.SetResolution(resolutions[resolutionDropDown.value].width, resolutions[resolutionDropDown.value].height, true);
        }
        else
        {
            Screen.SetResolution(resolutions[resolutionDropDown.value].width, resolutions[resolutionDropDown.value].height, false);
        }
    }

}
