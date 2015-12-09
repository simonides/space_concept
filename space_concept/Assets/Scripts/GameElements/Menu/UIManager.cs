using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyMessenger;

public class UIManager : MonoBehaviour 
{

    // ****    CONFIGURATION    **** //

    // ****  ATTACHED OBJECTS   **** //
    private PlanetMenuManager _planetMenuManager;
    private EventListManager _eventlistManager;
    // ****                     **** //

    public static UIManager instance;
    


    public List<PlanetEvent> events;
    public GameObject space;


    void Awake() {
        instance = this; // TODO this is hazardous!! if the ui manager is ever needed in the awake function nothing can guarantee the order and whether this has been initialized
               
         _planetMenuManager = this.GetComponent<PlanetMenuManager>();
        if (_planetMenuManager == null) {
            throw new MissingComponentException("Unable to find PlanetMenuManager.");
        }

        _eventlistManager = GetComponent<EventListManager>();
        if (_eventlistManager == null) {
            throw new MissingComponentException("Unable to find EventListManager.");
        }
    }





    public void ShowEventList()
    {
        ShowEventList(events);
        MessageHub.Publish(new MenuActiveEvent(this, true));// todo remove
    }


    public void ShowEventList(List<PlanetEvent> events)
    {
        _eventlistManager.activeEventlist = events;
        _eventlistManager.SwitchToEventlist();
    }


    public void PlanetClicked(Planet planet)
    {
        MessageHub.Publish(new MenuActiveEvent(this, true));// todo remove
        _planetMenuManager.ActivePlanet = planet.planetData;
        _planetMenuManager.SwitchToFirstLevel();
    }

    public void NextDayClicked() {
        MessageHub.Publish(new NextDayRequestEvent(this));
    }


    public void SendSomeSpaceShips() {
        Planet[] planets = space.GetComponentsInChildren<Planet>();



        //TODO: This is a test method. Remove it when it is not needed anymore!
        MessageHub.Publish(new NewTroopMovementEvent(this, planets[0], planets[1], 42));
      
    }

    public void HidePlanetMenus()
    {
        _planetMenuManager.SetPlanetMenuInVisible();
        MessageHub.Publish(new MenuActiveEvent(this, false));// todo remove
    }

    public void HideAllMenus()
    {
        _planetMenuManager.SetPlanetMenuInVisible();
        _eventlistManager.SetEventMenuInvisible();
        MessageHub.Publish(new MenuActiveEvent(this, false));// todo remove
    }

}
