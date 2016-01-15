

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class PauseMenuManager : AbstractMenuManager
{
    public Menu PauseMenu;

    private bool isMenuActive = false;

    void Awake()
    {
        MessageHub.Subscribe<ShowPauseMenuEvent>(ShowOptionsMenu);
        MessageHub.Subscribe<HidePauseMenuEvent>(HideOptionsMenu);
        MessageHub.Subscribe<ESCKeyPressedEvent>(ESCKeyPressed);
    }



    public void ShowOptionsMenu(ShowPauseMenuEvent event_)
    {
        isMenuActive = true;
        MessageHub.Publish(new AutoSaveGameEvent(this));
        SwitchMenu(PauseMenu);
    }

    public void HideOptionsMenu(HidePauseMenuEvent event_)
    {
        isMenuActive = false;
        SwitchMenu(null);
    }

    private void ESCKeyPressed(ESCKeyPressedEvent event_)
    {
        if (isMenuActive)
        {
            HideOptionsMenu(null);
        }
        else
        {
            ShowOptionsMenu(null);
        }
    }
}
