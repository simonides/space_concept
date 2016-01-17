using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TinyMessenger;

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

    private TinyMessageSubscriptionToken PlanetClickedEventToken;
    private TinyMessageSubscriptionToken CancelPlanetMenuEventToken;
    private TinyMessageSubscriptionToken ChooseOtherPlanetEventToken;
    private TinyMessageSubscriptionToken CancelChooseOtherPlanetEventToken;
    private TinyMessageSubscriptionToken CancelSendShipsEventToken;
    private TinyMessageSubscriptionToken ShipsSentEventToken;

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
        Debug.Assert(
            PlanetClickedEventToken == null
            && CancelPlanetMenuEventToken == null 
            && ChooseOtherPlanetEventToken == null 
            && CancelChooseOtherPlanetEventToken == null 
            && CancelSendShipsEventToken == null 
            && ShipsSentEventToken == null);

        PlanetClickedEventToken = MessageHub.Subscribe<PlanetClickedEvent>(PlanetClicked);
        CancelPlanetMenuEventToken = MessageHub.Subscribe<CancelPlanetMenuEvent>(CancelPlanetMenu);
        ChooseOtherPlanetEventToken = MessageHub.Subscribe<ChooseOtherPlanetEvent>(ChooseOtherPlanet);
        CancelChooseOtherPlanetEventToken = MessageHub.Subscribe<CancelChooseOtherPlanetEvent>(CancelChooseOtherPlanet);
        CancelSendShipsEventToken = MessageHub.Subscribe<CancelSendShipsEvent>(CancelSendShips);
        ShipsSentEventToken = MessageHub.Subscribe<ShipsSentEvent>(ShipsSent);       
    }


    private void ShipsSent(ShipsSentEvent obj)
    {
        Debug.Assert(_activeMenu == 3);
        _activeMenu = 1;
        planetTwo.setSelected(false);
        //ShowPlanetMenu();
        CancelPlanetMenu(null);
    }

    private void CancelSendShips(CancelSendShipsEvent event_)
    {
        Debug.Assert(_activeMenu == 3);
        _activeMenu = 2;
        planetTwo.setSelected(false);
        ShowChoosePlanetMenu();

    }

    private void CancelChooseOtherPlanet(CancelChooseOtherPlanetEvent event_)
    {
        Debug.Assert(_activeMenu == 2);
        _activeMenu = 1;
        MessageHub.Publish(new TactileBackgroundStateEvent(this, false));
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
        planetOne.setSelected(false);
        SwitchMenu(null);
        MessageHub.Publish(new ToggleNextDayButtonEvent(this, true));
    }

    private void PlanetClicked(PlanetClickedEvent event_)
    {
        MessageHub.Publish(new ToggleNextDayButtonEvent(this, false));

        Debug.Assert(_activeMenu == 0 || _activeMenu == 2);

        switch (_activeMenu)
        {
            case 0:
                if(event_.Content.planetData.Owner == null || 
                    !event_.Content.planetData.Owner.IsHumanPlayer) {
                    MessageHub.Publish(new ClickedOnForeignPlanetEvent(this, event_.Content));
                    //return; //TODO !!! uncomment this is just to debug so every planet can be used to send ships from
                }
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
        planetOne.setSelected(true);
        _planetMenuFiller.UpdateInfo(planetOne.planetData);
        SwitchMenu(PlanetMenu);
    }

    private void ShowChoosePlanetMenu()
    {
        MessageHub.Publish(new TactileBackgroundStateEvent(this, true));
        SwitchMenu(ChoosePlanetMenu);
        MessageHub.Publish(new MenuActiveEvent(this, false)); // reenable UI interaction
    }
    private void ShowSendShipsMenu()
    {
        Debug.Assert(planetOne != planetTwo);
        MessageHub.Publish(new TactileBackgroundStateEvent(this, false));
        planetTwo.setSelected(true);
        _sendShipMenuFiller.UpdateUI(planetOne, planetTwo);
        SwitchMenu(SendShipsMenu);
    }

    // close the current menu
    public void SetPlanetMenuInVisible()
    {
        SwitchMenu(null);
    }


    void OnDestroy()
    {
        MessageHub.Unsubscribe<PlanetClickedEvent>(PlanetClickedEventToken);
        MessageHub.Unsubscribe<CancelPlanetMenuEvent>(CancelPlanetMenuEventToken);
        MessageHub.Unsubscribe<ChooseOtherPlanetEvent>(ChooseOtherPlanetEventToken);
        MessageHub.Unsubscribe<CancelChooseOtherPlanetEvent>(CancelChooseOtherPlanetEventToken);
        MessageHub.Unsubscribe<CancelSendShipsEvent>(CancelSendShipsEventToken);
        MessageHub.Unsubscribe<ShipsSentEvent>(ShipsSentEventToken);
    }



}
