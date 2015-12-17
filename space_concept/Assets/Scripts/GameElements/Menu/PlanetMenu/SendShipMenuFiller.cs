using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SendShipMenuFiller : MonoBehaviour {

    public Text PlanetNameOne;
    public Text TotalShips;
    public Text FromShips;
    public Text ToShips;
    public Slider slider;
    public Text PlanetNameTwo;
    public Text TravelDistance;

    private Planet planetOne;
    private Planet planetTwo;

    void Awake()
    {
        MessageHub.Subscribe<SendShipsEvent>(SendShips);
    }

    public void UpdateUI(Planet planetOne, Planet planetTwo)
    {
       this.planetOne = planetOne;
       this.planetTwo = planetTwo;

        TravelDistance.text = ""+ ((int)planetOne.planetData.GetSurfaceDistance(planetTwo.planetData));

        //slider.minValue = 0;
        slider.maxValue =  planetOne.planetData.Ships;
        slider.value = (int)planetOne.planetData.Ships * 0.5f;
        PlanetNameOne.text = planetOne.planetData.Name;
        PlanetNameTwo.text = planetTwo.planetData.Name;
        TotalShips.text = planetOne.planetData.Ships.ToString();
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
        }
        else
        {
            Debug.Log("Not enough ships to send!");
        }
    }

    }
