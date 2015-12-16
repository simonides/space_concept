using UnityEngine;
using System.Collections;

using Custom.Utility;
using Custom.Base;

public class SettingMenuController : MonoBehaviour {

    public UnityEngine.UI.Slider masterSlider;
    public UnityEngine.UI.Slider musicSlider;
    public UnityEngine.UI.Slider sfxslider;

    void Start() {
        
    }
    void Awake()
    {
        masterSlider.value = SettingsController.GetInstance().dataFile.masterVolume;
        musicSlider.value = SettingsController.GetInstance().dataFile.musicVolume;
        sfxslider.value = SettingsController.GetInstance().dataFile.sfxVolume;
    }

    public void Button_LoadScene(string f_sceneName)
    {
        SaveSettings();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(f_sceneName);
    }
    public void Button_LoadScene(int f_sceneIndex)
    {
        SaveSettings();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(f_sceneIndex);

    }

    public void SaveSettings()
    {
        SettingsController.GetInstance().dataFile.masterVolume = masterSlider.value;
        SettingsController.GetInstance().dataFile.musicVolume = musicSlider.value;
        SettingsController.GetInstance().dataFile.sfxVolume = sfxslider.value;
        SettingsController.GetInstance().SaveData();
    }

}
