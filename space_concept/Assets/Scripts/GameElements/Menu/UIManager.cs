using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyMessenger;
using System;

public class UIManager : MonoBehaviour 
{

    // ****    CONFIGURATION    **** //

    // ****  ATTACHED OBJECTS   **** //
    private PlanetMenuManager _planetMenuManager;
    //private EventListManager _eventlistManager;
    // ****                     **** //

    public static UIManager instance;

    public List<PlanetEvent> events;


    void Awake() {
        instance = this; // TODO this is hazardous!! if the ui manager is ever needed in the awake function nothing can guarantee the order and whether this has been initialized
               
         _planetMenuManager = this.GetComponent<PlanetMenuManager>();
        if (_planetMenuManager == null) {
            throw new MissingComponentException("Unable to find PlanetMenuManager.");
        }

        MessageHub.Subscribe<CancelSendShipsEvent>(CancelSendShips);

    }

    private void CancelSendShips(CancelSendShipsEvent obj)
    {
        //todo!!!!!!!!!!!!!!!!!!!!!!!!!!!
        _planetMenuManager.SwitchToFirstLevel();
    }

    public void PlanetClicked(Planet planet)
    {
        MessageHub.Publish(new MenuActiveEvent(this, true));// todo remove
        _planetMenuManager.ActivePlanet = planet.planetData;
        _planetMenuManager.SwitchToFirstLevel();
    }


    public void HidePlanetMenus()
    {
        _planetMenuManager.SetPlanetMenuInVisible();
        MessageHub.Publish(new MenuActiveEvent(this, false));// todo remove
    }

    public void HideAllMenus()
    {
        _planetMenuManager.SetPlanetMenuInVisible();
        //_eventlistManager.SetEventMenuInvisible();
        MessageHub.Publish(new MenuActiveEvent(this, false));// todo remove
    }

}
