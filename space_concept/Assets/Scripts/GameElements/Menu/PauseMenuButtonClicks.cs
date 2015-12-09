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
    }

    public void CloseInGameMenu()
    {
        MessageHub.Publish(new HideOptionsMenu(this));
    }
}
