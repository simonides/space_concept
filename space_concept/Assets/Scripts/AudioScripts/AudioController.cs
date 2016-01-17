using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Custom.Utility;
using Custom.Base;

public class AudioController : SingletonBase<AudioController> {
    public AudioSource menuClick;
    public AudioSource menuClickDenied;
    public AudioSource planetSelection;
    public AudioSource planetSelectionDenied;
    public AudioSource openMenu;
    public AudioSource closeMenu;
    public AudioSource nextDayNothingHappend;
    public AudioSource nextDaySomethingHappend;

    public enum SoundCodes {MenuClick, MenuClickDenied, PlanetSelection, PlanetSelectionDenied, OpenMenu, CloseMenu, NextDayNothing, NextDaySomething};
    private  Dictionary<SoundCodes, AudioSource> sounds;
    private  List<AudioSource> sources;

    private bool initOnce = false;
    private static AudioController ac;
    [Header("These Enum names are mapped to the provided sounds. first element has int value = 0")]
    public SoundCodes AudioEnumCodes;

    protected override void Awake() {
        base.Awake(this);
        init();
      
    }
    // Use this for initialization
    void Start() {
      
    }

    // Update is called once per frame
    void Update() {

    }

    public void init()
    {
        if (!initOnce)
        {
            initOnce = true;
            ac = GetInstance();
            sounds = new Dictionary<SoundCodes, AudioSource>();
            sources = new List<AudioSource>();

            sounds.Add(SoundCodes.MenuClick, menuClick);
            sounds.Add(SoundCodes.MenuClickDenied, menuClickDenied);
            sounds.Add(SoundCodes.PlanetSelection, planetSelection);
            sounds.Add(SoundCodes.PlanetSelectionDenied, planetSelectionDenied);
            sounds.Add(SoundCodes.OpenMenu, openMenu);
            sounds.Add(SoundCodes.CloseMenu, closeMenu);
            sounds.Add(SoundCodes.NextDayNothing, nextDayNothingHappend);
            sounds.Add(SoundCodes.NextDaySomething, nextDaySomethingHappend);

            menuClick = null;
            menuClickDenied = null;
            planetSelection = null;
            planetSelectionDenied = null;
            openMenu = null;
            closeMenu = null;
            nextDayNothingHappend = null;
            nextDaySomethingHappend = null;

        }
    }
    //10^(x / 20) = y
    //convert a float value of DB into a linear form in float
    public static float DBToFloat(float db) {
        float result = Mathf.Pow(10, (db / 20));
        return result;
    }
    //x=20*Log10(y)
    //convert a linear float value into a DB representation
    public static float FloatToDB(float volume) {
        volume = NormalizeFloat(volume);
        if (volume <= 0) {
            volume = 0.0001f;
        }
        float result = 20 * Mathf.Log10(volume);

        return result;
    }

    //normalize to a max value of 100 and min of 0;
    public static float NormalizeFloat(float volume) {
        float max = 100.0f;
        if (volume > max) {
            volume = max;
        }
        float result = volume / max;
        return result;
    }

    //find a pre existing available audiosource to play a sound
    //creats more audiosources if no free object is found
    //dosent clear the list since the objects are reused
    private int GetEmptyAudioSourceObject()
    {
        
        int size = sources.Count;
        //find audiosource
        for(int i = 0; i < size; i++){
            //find null object
            if (sources[i] == null)
            {
                return i;
            }
            //find object that is not playing
            if (sources[i].isPlaying == false)
            {
                return i;
            }
        }
        //add new object
        AudioSource s = null;
        sources.Add(s);
        return size;

    }

    public void PlayMenuClickSound() {
        ac.SelectSoundToPlay(SoundCodes.MenuClick);
    }

    public void PlaySound(int code) {
        ac.SelectSoundToPlay((SoundCodes)code);
    }

    private void SelectSoundToPlay(SoundCodes code)
    {
        int i =  GetEmptyAudioSourceObject();
        AudioSource s = null;
        sounds.TryGetValue(code, out s);
        sources[i] = s;
        if (!sources[i].isPlaying)
        {
            sources[i].Play();
        }
    }
}
