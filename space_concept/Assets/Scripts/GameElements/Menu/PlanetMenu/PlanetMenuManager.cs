using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlanetMenuManager : AbstractMenuManager
{

    //public PlanetData ActivePlanet { get; set; }

    public Menu PlanetMenu;
    public Menu SendShipsMenu;
    public Menu ChoosePlanetMenu;

    enum MenuState
    {
        NONE, PLANET, CHOOSE, SEND 
    }

    int _activeMenu = 0;
    private Planet planetOne;
    private Planet planetTwo;

    private PlanetMenuFiller _planetMenuFiller;
    private SendShipMenuFiller _sendShipMenuFiller;

    void Awake()
    {
        _planetMenuFiller = GetComponentInChildren<PlanetMenuFiller>();
        if (_planetMenuFiller == null) {
            throw new MissingComponentException("Unable to find PlanetMenuFiller.");
        }

        _sendShipMenuFiller = GetComponentInChildren<SendShipMenuFiller>();
        if (_sendShipMenuFiller == null){
            throw new MissingComponentException("Unable to find _sendShipMenuFiller.");
        }

        MessageHub.Subscribe<PlanetClickedEvent>(PlanetClicked);
        MessageHub.Subscribe<CancelPlanetMenuEvent>(CancelPlanetMenu);
        MessageHub.Subscribe<ChooseOtherPlanetEvent>(ChooseOtherPlanet);
        MessageHub.Subscribe<CancelChooseOtherPlanetEvent>(CancelChooseOtherPlanet);
        MessageHub.Subscribe<CancelSendShipsEvent>(CancelSendShips);
    }

    private void CancelSendShips(CancelSendShipsEvent event_)
    {
        Debug.Assert(_activeMenu == 3);
        _activeMenu = 2;
        planetTwo.RemoveGlow();
        ShowChoosePlanetMenu();
    }

    private void CancelChooseOtherPlanet(CancelChooseOtherPlanetEvent event_)
    {
        Debug.Assert(_activeMenu == 2);
        _activeMenu = 1;
        ShowPlanetMenu();
    }

    private void ChooseOtherPlanet(ChooseOtherPlanetEvent event_)
    {
        Debug.Assert(_activeMenu == 1);
        _activeMenu = 2;
        ShowChoosePlanetMenu();
    }

    private void CancelPlanetMenu(CancelPlanetMenuEvent event_)
    {
        _activeMenu = 0;
        planetOne.RemoveGlow();
        SwitchMenu(null);
    }

    private void PlanetClicked(PlanetClickedEvent event_)
    {
        Debug.Assert(_activeMenu == 0 || _activeMenu == 2);

        switch (_activeMenu)
        {
            case 0:

                planetOne = event_.Content;
                ShowPlanetMenu();
                break;
            case 1:
                Debug.Log("Error in PlanetMenuManager: Planet clicked when it should not be possible.");
                throw new Exception("Planets should not be clickable when the planet menu is active.");
            case 2:
                if(planetOne == event_.Content){
                    Debug.Log("Choose an other planet, this is the planet you try to send ships from");
                    return;
                }
                planetTwo = event_.Content;
                ShowSendShipsMenu();
                break;

            default:
                break;
        }
        ++_activeMenu;

    }


    private void ShowPlanetMenu()
    {
        Debug.Assert(planetOne != null);
        planetOne.SetGlow();
        _planetMenuFiller.UpdateInfo(planetOne.planetData);
        SwitchMenu(PlanetMenu);
    }

    private void ShowChoosePlanetMenu()
    {
        SwitchMenu(ChoosePlanetMenu);
        MessageHub.Publish(new MenuActiveEvent(this, false)); // reenable UI interaction
    }
    private void ShowSendShipsMenu()
    {
        Debug.Assert(planetOne != planetTwo);
        planetTwo.SetGlow();
        _sendShipMenuFiller.UpdateUI(planetOne, planetTwo);
        SwitchMenu(SendShipsMenu);
    }

    // close the current menu
    public void SetPlanetMenuInVisible()
    {
        SwitchMenu(null);
    }
}
