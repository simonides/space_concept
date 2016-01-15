using UnityEngine;
using System.Collections;

public class AlwaysOnButtons : MonoBehaviour {

    public void NextDay() {
        MessageHub.Publish(new NextDayRequestEvent(this));
    }

    public void ShowEventList(){
        MessageHub.Publish(new ShowEventListEvent(this));
    }
}
