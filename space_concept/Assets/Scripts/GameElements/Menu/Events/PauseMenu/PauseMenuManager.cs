using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using TinyMessenger;

public class PauseMenuManager : AbstractMenuManager
{
    public Menu PauseMenu;

    private bool isMenuActive = false;
    private TinyMessageSubscriptionToken ShowPauseMenuEventToken;
    private TinyMessageSubscriptionToken HidePauseMenuEventToken;
    private TinyMessageSubscriptionToken ESCKeyPressedEventToken;

    void Awake(){
        Debug.Assert(ShowPauseMenuEventToken == null 
            && HidePauseMenuEventToken == null 
            && ESCKeyPressedEventToken == null);
        ShowPauseMenuEventToken = MessageHub.Subscribe<ShowPauseMenuEvent>(ShowOptionsMenu);
        HidePauseMenuEventToken = MessageHub.Subscribe<HidePauseMenuEvent>(HideOptionsMenu);
        ESCKeyPressedEventToken = MessageHub.Subscribe<ESCKeyPressedEvent>(ESCKeyPressed);
    }



    public void ShowOptionsMenu(ShowPauseMenuEvent event_){
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


    void OnDestroy(){
        MessageHub.Unsubscribe<ShowPauseMenuEvent>(ShowPauseMenuEventToken);
        MessageHub.Unsubscribe<HidePauseMenuEvent>(HidePauseMenuEventToken);
        MessageHub.Unsubscribe<ESCKeyPressedEvent>(ESCKeyPressedEventToken);
    }
}
