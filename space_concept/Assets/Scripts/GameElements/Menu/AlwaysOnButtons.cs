using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class AlwaysOnButtons : MonoBehaviour {

    public Button NextDayButton;

    void Awake(){
        MessageHub.Subscribe<ToggleNextDayButtonEvent>(ToggleNextDayButton);
    }

    private void ToggleNextDayButton(ToggleNextDayButtonEvent obj){
        Debug.Log("Nextday button active: " + obj.Content);
        NextDayButton.enabled = obj.Content;
    }

    public void NextDay() {
        MessageHub.Publish(new NextDayRequestEvent(this));
    }

    public void ShowEventList(){
        MessageHub.Publish(new ShowEventListEvent(this));
    }
}
