using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TinyMessenger;

public class AlwaysOnButtons : MonoBehaviour {

    public Button NextDayButton;
    private TinyMessageSubscriptionToken ToggleNextDayButtonEventToken;
    void Awake(){
        Debug.Assert(ToggleNextDayButtonEventToken == null);
        ToggleNextDayButtonEventToken = MessageHub.Subscribe<ToggleNextDayButtonEvent>(ToggleNextDayButton);
    }

    private void ToggleNextDayButton(ToggleNextDayButtonEvent obj){
        Debug.Log("Nextday button active: " + obj.Content);
        NextDayButton.interactable = obj.Content;
    }

    public void NextDay() {
        MessageHub.Publish(new NextDayRequestEvent(this));
    }

    public void ShowEventList(){
        MessageHub.Publish(new ShowEventListEvent(this));
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe < ToggleNextDayButtonEvent>(ToggleNextDayButtonEventToken);
    }
}
