using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

public class EventListManager : MonoBehaviour {


    public Menu eventListMenu;

    private Menu _currentMenu;
    private EventListFiller _eventListFiller;

    public List<PlanetEvent> activeEventlist;


    void Awake()
    {
        _eventListFiller = GetComponentInChildren<EventListFiller>();
    }

    public void SwitchToEventlist()
    {
        _eventListFiller.Fill(activeEventlist);
        SwitchMenu(eventListMenu);
    }

    private void SwitchMenu(Menu menu)
    {
        if (_currentMenu != null)
        {
            _currentMenu.IsOpen = false;
        }
        _currentMenu = menu;
        _currentMenu.IsOpen = true;
    }


    // close the current menu
    public void SetEventMenuInvisible()
    {
        if (_currentMenu != null){
            _currentMenu.IsOpen = false;
        }
    }
}
