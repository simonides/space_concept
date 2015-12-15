using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MasterMixerController : MonoBehaviour {

    public AudioMixer master;


    public void SetVoulume(float volume)
    {
        master.SetFloat("MasterVolume", AudioController.FloatToDB(volume));//actually needs the float value as DB to correctly work.... float = 1 ==> db = 0
    }

    //gives controll over exposed parameter back to snapshots
    public void ClearVolume()
    {
        master.ClearFloat("MasterVolume");

    }

    public float GetVolume()
    {
        float result;
        master.GetFloat("MasterVolume", out result);
        return result;
    }

    void Start()
    {
        float volume = SettingsController.GetInstance().dataFile.masterVolume;
        SetVoulume(volume);
    }
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
