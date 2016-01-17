using UnityEngine;
using System.Collections;

using Custom.Utility;
using Custom.Base;

public class SettingMenuController : MonoBehaviour {

    public UnityEngine.UI.Slider fogOfWarSlider;
    public UnityEngine.UI.Slider masterSlider;
    public UnityEngine.UI.Slider musicSlider;
    public UnityEngine.UI.Slider sfxslider;

    public UnityEngine.UI.Text fogDist;

    private SettingsController manager;
    void Start() {
        manager = SettingsController.GetInstance();
        masterSlider.value = manager.dataFile.masterVolume;
        musicSlider.value = manager.dataFile.musicVolume;
        sfxslider.value = manager.dataFile.sfxVolume;

        fogOfWarSlider.value = manager.dataFile.fogDist;
    }
    void Awake()
    {
       
      
       
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
        
        manager.dataFile.masterVolume = masterSlider.value;
        manager.dataFile.musicVolume = musicSlider.value;
        manager.dataFile.sfxVolume = sfxslider.value;
        manager.SaveData();
    }

    public void Slider_OnChange(UnityEngine.UI.Slider slider)
    {
        fogDist.text = slider.value.ToString();
        SettingsController.GetInstance().dataFile.fogDist = (int)slider.value;
    }

}
