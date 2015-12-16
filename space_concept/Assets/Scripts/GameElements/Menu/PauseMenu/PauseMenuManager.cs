

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



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
        MessageHub.Publish(new AutoSaveGameEvent(this));
        SwitchMenu(PauseMenu);
    }

    public void HideOptionsMenu(HidePauseMenuEvent event_)
    {
        SwitchMenu(null);
    }

    private void ESCKeyPressed(ESCKeyPressedEvent event_)
    {
        isMenuActive = !isMenuActive;
        if (isMenuActive)
        {
            ShowOptionsMenu(null);
        }
        else
        {
            HideOptionsMenu(null);
        }
    }
}
