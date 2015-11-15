using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour 
{

    public static UIManager instance;

    private PlanetMenuManager _planetMenuManager;
    private EventListManager _eventlistManager;

    public List<PlanetEvent> events;
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


    public void HidePlanetMenus()
    {
        _planetMenuManager.SetPlanetMenuInVisible();
    }

    public void HideAllMenus()
    {
        _planetMenuManager.SetPlanetMenuInVisible();
        _eventlistManager.SetEventMenuInvisible();
    }

    void Awake()
    {
        instance = this; // TODO this is hazardous!! if the ui manager is ever needed in the awake function nothing can guarantee the order and whether this has been initialized
        _planetMenuManager = this.GetComponent<PlanetMenuManager>();
        _eventlistManager = GetComponent<EventListManager>();
    }

}
