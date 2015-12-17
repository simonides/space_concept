using UnityEngine;
using System.Collections;

public class PlanetMenuButtonClicks : MonoBehaviour {


    public void UpgradeFactory()
    {

    }

    public void UpgradeHangar()
    {

    }

    public void SendShips()
    {
        MessageHub.Publish(new ChooseOtherPlanetEvent(this));
    }

    public void CloseMenu()
    {
        MessageHub.Publish(new CancelPlanetMenuEvent(this));
    }

    public void CancelOtherPlanetChoosing()
    {
        MessageHub.Publish(new CancelChooseOtherPlanetEvent(this));
    }
}
