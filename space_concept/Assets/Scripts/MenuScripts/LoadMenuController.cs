using UnityEngine;
using System.Collections;

public class LoadMenuController : MonoBehaviour {
    public void Button_LoadScene(string f_sceneName)
    {
        Application.LoadLevel(f_sceneName);
    }
    public void Button_LoadScene(int f_sceneIndex)
    {
        Application.LoadLevel(f_sceneIndex);
    }

    public void Button_LoadSaveGame(string f_saveGameName) {
        //DO SOMETHING
    }
}
