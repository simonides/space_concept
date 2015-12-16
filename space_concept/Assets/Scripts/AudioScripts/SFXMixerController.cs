using UnityEngine;
using System.Collections;

using UnityEngine.Audio;
public class SFXMixerController : MonoBehaviour {

    public AudioMixer master;

    public void SetVoulume(float volume)
    {
        master.SetFloat("SFXVolume", AudioController.FloatToDB(volume));
    }

    //gives controll over exposed parameter back to snapshots
    public void ClearVolume()
    {
        master.ClearFloat("SFXVolume");

    }

    public float GetVolume()
    {
        float result;
        master.GetFloat("SFXVolume", out result);
        return result;
    }

    void Start()
    {
        float volume = SettingsController.GetInstance().dataFile.sfxVolume;
        SetVoulume(volume);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
