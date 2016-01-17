using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TinyMessenger;

public class SendShipMenuFiller : MonoBehaviour
{

    public Text PlanetNameOne;
    public Text FromShips;
    public Text ToShips;
    public Slider slider;
    public Text PlanetNameTwo;
    public Text TravelDistance;

    private Planet planetOne;
    private Planet planetTwo;

    private GameState gameState;
    private TinyMessageSubscriptionToken SendShipsEventToken;
    void Awake()
    {
        Debug.Assert(SendShipsEventToken == null);
        SendShipsEventToken = MessageHub.Subscribe<SendShipsEvent>(SendShips);

        gameState = GameObject.Find("2D_MainCam").GetComponent<GameState>();


        if (gameState == null)
        {
            throw new MissingComponentException("Unable to find the GameState. It should be part of the '2D_MainCam'.");
        }

    }

    public void UpdateUI(Planet planetOne, Planet planetTwo)
    {
        this.planetOne = planetOne;
        this.planetTwo = planetTwo;
        var travelTime = TroopData.GetTravelTime(planetOne.planetData, planetTwo.planetData);
        TravelDistance.text = "" + travelTime
            + " Ship arrives on day: " + (gameState.gameStateData.CurrentDay + travelTime);

        slider.maxValue = planetOne.planetData.Ships;
        slider.value = (int)planetOne.planetData.Ships * 0.5f;
        PlanetNameOne.text = planetOne.planetData.Name;
        PlanetNameTwo.text = planetTwo.planetData.Name;
        OnSliderValueChanged();
    }

    public void OnSliderValueChanged()
    {
        FromShips.text = (planetOne.planetData.Ships - slider.value).ToString();
        ToShips.text = slider.value.ToString();
    }


    private void SendShips(SendShipsEvent event_)
    {
        if (slider.value > 0)
        {
            MessageHub.Publish(new NewTroopMovementEvent(this, planetOne, planetTwo, (int)slider.value));
            MessageHub.Publish(new ShipsSentEvent(this));
        }
        else
        {
            Debug.Log("Not enough ships to send!");
        }
    }
    void OnDestroy(){
        MessageHub.Unsubscribe<SendShipsEvent>(SendShipsEventToken);
    }
}
