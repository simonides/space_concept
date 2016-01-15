using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayIndicatorUpdater : MonoBehaviour {

    UnityEngine.UI.Text currentDayText;
    
    void Awake() {
        //playerManager = this.game1
        currentDayText = this.transform.FindChild("CurrentDayText").GetComponent<UnityEngine.UI.Text>();
        if (currentDayText == null) {
            throw new MissingComponentException("Unable to find currentDayText. This ui-text component must be attached to the 'CurrentDayText' GameObject in the canvas.");
        }
    }
    void Start() {
        MessageHub.Subscribe<NextDayEvent>((NextDayEvent evt) => UpdateText(evt.GetCurrentDay()));
    }

    public void UpdateText(int currentDay) {

        currentDayText.text = ""+currentDay;
    }


    void Update() {

    }
}
