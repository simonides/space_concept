using UnityEngine;
using System.Collections;

using UnityEngine.Audio;


public class MusicMixerController : MonoBehaviour {

    public AudioMixer master;

    public void SetVoulume(float volume)
    {
        master.SetFloat("AmbientVolume", AudioController.FloatToDB(volume));
    }

    //gives controll over exposed parameter back to snapshots
    public void ClearVolume()
    {
        master.ClearFloat("AmbientVolume");

    }

    public float GetVolume()
    {
        float result;
        master.GetFloat("AmbientVolume", out result);
        return result;
    }

    void Start()
    {
        float volume = SettingsController.GetInstance().dataFile.musicVolume;
        SetVoulume(volume);
    
    }

    // Update is called once per frame
    void Update()
    {

    }
}
