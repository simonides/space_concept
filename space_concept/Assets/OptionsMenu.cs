using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {



    public void SaveGame()
    {
        MessageHub.Publish(new SaveGameEvent(this,"test"));
    }

    public void QuitGame()
    {
        MessageHub.Publish(new QuitGameEvent(this));
    }

    public void CloseInGameMenu(new CloseInGameMenu(this);)
}
