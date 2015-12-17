using UnityEngine;
using System.Collections;

public class AlwaysOnButtons : MonoBehaviour {

    public void NextDay()
    {
        MessageHub.Publish(new NextDayRequestEvent(this));
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    }
}
