using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TinyMessenger;

public class DayIndicatorUpdater : MonoBehaviour {

    UnityEngine.UI.Text currentDayText;
    //int currentlyDisplayedDay

    
    void Awake() {
        //playerManager = this.game1
        currentDayText = this.transform.FindChild("CurrentDayText").GetComponent<UnityEngine.UI.Text>();
        if (currentDayText == null) {
            throw new MissingComponentException("Unable to find currentDayText. This ui-text component must be attached to the 'CurrentDayText' GameObject in the canvas.");
        }
    }

    TinyMessageSubscriptionToken NextDayEventSubscription;
    void Start() {
        GameState gameState = GameObject.Find("2D_MainCam").GetComponent<GameState>();
        if (gameState == null) {
            throw new MissingComponentException("Unable to find the GameState. It should be part of the '2D_MainCam'.");
        }
        currentDayText.text = "" + gameState.gameStateData.CurrentDay;
        Debug.Assert(NextDayEventSubscription == null);
        NextDayEventSubscription = MessageHub.Subscribe<NextDayEvent>((NextDayEvent evt) => UpdateText(evt.GetCurrentDay()));       
    }

    void OnDestroy() {
        MessageHub.Unsubscribe<NextDayEvent>(NextDayEventSubscription);
    }

    public void UpdateText(int currentDay) {
        currentDayText.text = ""+currentDay;
    }


    void Update() {

    }
}
