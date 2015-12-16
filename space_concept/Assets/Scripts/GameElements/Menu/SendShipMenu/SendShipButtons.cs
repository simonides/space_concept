using UnityEngine;

public class SendShipButtons : MonoBehaviour
{

    public void CancelSendShips()
    {
        MessageHub.Publish(new CancelSendShipsEvent(this));
    }

    public void SendShips()
    {
        //MessageHub.Publish(new SendShipsEvent(this));
    }
}
