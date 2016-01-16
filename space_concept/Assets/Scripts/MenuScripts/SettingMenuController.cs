using UnityEngine;
using System.Collections;

using Custom.Utility;
using Custom.Base;

public class SettingMenuController : MonoBehaviour {

    public UnityEngine.UI.Toggle fogOfWarToggle;
    public UnityEngine.UI.Slider masterSlider;
    public UnityEngine.UI.Slider musicSlider;
    public UnityEngine.UI.Slider sfxslider;

    private SettingsController manager;
    void Start() {
        
    }
    void Awake()
    {
        manager = SettingsController.GetInstance();
        fogOfWarToggle.isOn = manager.dataFile.fogOfWar;
        masterSlider.value = manager.dataFile.masterVolume;
        musicSlider.value = manager.dataFile.musicVolume;
        sfxslider.value = manager.dataFile.sfxVolume;
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
        manager.dataFile.fogOfWar = fogOfWarToggle.isOn;
        manager.dataFile.masterVolume = masterSlider.value;
        manager.dataFile.musicVolume = musicSlider.value;
        manager.dataFile.sfxVolume = sfxslider.value;
        manager.SaveData();
    }

}
