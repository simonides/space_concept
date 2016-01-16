using UnityEngine;
using System.Collections;

public class PauseMenuButtonClicks : MonoBehaviour {


    public void SaveGame()
    {
        MessageHub.Publish(new SaveGameEvent(this, "test"));
    }

    public void QuitGame()
    {
        MessageHub.Publish(new QuitGameEvent(this));
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void CloseInGameMenu()
    {
        MessageHub.Publish(new HidePauseMenuEvent(this));
    }
}
