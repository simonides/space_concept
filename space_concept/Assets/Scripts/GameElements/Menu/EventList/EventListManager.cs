using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Item
{
    public string name;
    public Sprite icon;
    public string type;
    public string rarity;
    public bool isChampion;
    public Button.ButtonClickedEvent thingToDo;
}

[System.Serializable]
public class PlanetEvent{
    public string planetName;
}

public class EventListManager : AbstractMenuManager {


    public Menu eventListMenu;
    private EventListFiller _eventListFiller;
    public List<PlanetEvent> activeEventlist;


    void Awake()
    {
        _eventListFiller = GetComponentInChildren<EventListFiller>();
        MessageHub.Subscribe<ShowEventListEvent>(ShowEventList);
        MessageHub.Subscribe<HideEventListEvent>(HideEventList);
    }



    public void ShowEventList(ShowEventListEvent event_)
    {
        _eventListFiller.Fill(activeEventlist);
        SwitchMenu(eventListMenu);
    }

    public void HideEventList(HideEventListEvent event_)
    {
        SwitchMenu(null);
    }
}
