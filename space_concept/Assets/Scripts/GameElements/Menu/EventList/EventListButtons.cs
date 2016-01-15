using UnityEngine;
using System.Collections;

public class EventListButtons : MonoBehaviour
{

    public void Close()
    {
        MessageHub.Publish(new HideEventListEvent(this));
    }
}
