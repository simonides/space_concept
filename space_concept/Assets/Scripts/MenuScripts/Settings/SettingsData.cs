using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

[System.Serializable]
public class SettingsData  {

    public bool fogOfWar = true;
    public float masterVolume = 100.0f;
    public float musicVolume = 100.0f;
    public float sfxVolume = 100.0f;
}
