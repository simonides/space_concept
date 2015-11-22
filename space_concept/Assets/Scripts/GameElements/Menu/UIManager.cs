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
    }


    public void ShowEventList(List<PlanetEvent> events)
    {
        _eventlistManager.activeEventlist = events;
        _eventlistManager.SwitchToEventlist();
    }


    public void PlanetClicked(Planet planet)
    {
        _planetMenuManager.ActivePlanet = planet.planetData;
        _planetMenuManager.SwitchToFirstLevel();
    }

    public void NextDayClicked() {
        MessageHub.Publish(new NextDayRequest(this));
    }

    public void HidePlanetMenus()
    {
        _planetMenuManager.SetPlanetMenuInVisible();
    }

    public void HideAllMenus()
    {
        _planetMenuManager.SetPlanetMenuInVisible();
        _eventlistManager.SetEventMenuInvisible();
    }

}
