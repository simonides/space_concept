using UnityEngine;
using System.Collections;

public class SettingMenuController : MonoBehaviour {

    public void Button_LoadScene(string f_sceneName)
    {
        Application.LoadLevel(f_sceneName);
    }
    public void Button_LoadScene(int f_sceneIndex)
    {
        Application.LoadLevel(f_sceneIndex);
    }

}
