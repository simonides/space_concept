using UnityEngine;
using System.Collections;

public class ButtonListenerForDebuging : MonoBehaviour {

    public void NextDay()
    {

    }

    public void SendSomeShips()
    {

    }

    public void ShowEventList()
    {
       // MessageHub.Publish(new ShowEventListEvent(this));
    }

    public void HideEventList()
    {
       // MessageHub.Publish(new HideEventListEvent(this));
    }

    public void OptionMenu()
    {
        MessageHub.Publish(new ShowOptionsMenu(this));
    }
}
