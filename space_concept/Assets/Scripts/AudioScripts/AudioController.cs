using UnityEngine;
using System.Collections;

using Custom.Utility;
using Custom.Base;

public class AudioController : SingletonBase<AudioController> {
    public AudioSource menuClick;

    void Awake()
    {
        base.Awake(this);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //10^(x / 20) = y
    //convert a float value of DB into a linear form in float
    public static float DBToFloat(float db){
        float result = Mathf.Pow(10, (db / 20));
        return result;
    }
    //x=20*Log10(y)
    //convert a linear float value into a DB representation
    public static float FloatToDB(float volume)
    {
        volume = NormalizeFloat(volume);
        if (volume <= 0)
        {
            volume = 0.0001f;
        }
        float result = 20*Mathf.Log10(volume);
        
        return result;
    }

    //normalize to a max value of 100 and min of 0;
    public static float NormalizeFloat(float volume)
    {
        float max = 100.0f;
        if(volume > max){
            volume = max;
        }
        float result = volume/max;
        return result;
    }

    public void PlayMenuClickSound()
    {
        //AudioController.GetInstance().menuClick.enabled = true;
        AudioController.GetInstance().menuClick.Play();
    }

    
}
