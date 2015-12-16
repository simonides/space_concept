using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void Button_LoadScene(string f_sceneName){
       // Application.LoadLevel(f_sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(f_sceneName);
        if (f_sceneName == "Game")
        {
            AnimatedBackgroundDontDestroy.GetInstance().DestroyThis();
        }
    }
    public void Button_LoadScene(int f_sceneIndex){
       // Application.LoadLevel(f_sceneIndex);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(f_sceneIndex);
    }

    public void Button_Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
