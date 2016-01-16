using UnityEngine;
using System.Collections;

public class EndScreenButtonClick : MonoBehaviour {

    public void OnQuitToMenu(){
        MessageHub.Publish(new QuitGameEvent(this));
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
