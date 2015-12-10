

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
        MessageHub.Subscribe<ShowPauseMenu>(ShowOptionsMenu);
        MessageHub.Subscribe<HidePauseMenu>(HideOptionsMenu);
    }

    public void ShowOptionsMenu(ShowPauseMenu event_)
    {
        SwitchMenu(PauseMenu);
    }

    public void HideOptionsMenu(HidePauseMenu event_)
    {
        SwitchMenu(null);
    }
}
