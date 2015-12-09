

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class PauseMenuManager : AbstractMenuManager
{
    public Menu PauseMenu;

    void Awake()
    {
        MessageHub.Subscribe<ShowOptionsMenu>(ShowOptionsMenu);
        MessageHub.Subscribe<HideOptionsMenu>(HideOptionsMenu);
    }

    public void ShowOptionsMenu(ShowOptionsMenu event_)
    {
        SwitchMenu(PauseMenu);
    }

    public void HideOptionsMenu(HideOptionsMenu event_)
    {
        SwitchMenu(null);
    }
}
