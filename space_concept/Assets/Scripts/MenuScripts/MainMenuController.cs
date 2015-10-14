using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void Button_LoadScene(string f_sceneName){
        Application.LoadLevel(f_sceneName);
    }
    public void Button_LoadScene(int f_sceneIndex){
        Application.LoadLevel(f_sceneIndex);
    }

    public void Button_Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
