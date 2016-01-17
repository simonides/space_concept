using UnityEngine;
using System.Collections;
using System;
using TinyMessenger;

public class EndScreenManager : AbstractMenuManager
{

    public Menu EndScreenMenu;
    private TinyMessageSubscriptionToken GameFinishedEventToken;

    void Awake(){
        GameFinishedEventToken = MessageHub.Subscribe<GameFinishedEvent>(GameFinished);
    }

    private void GameFinished(GameFinishedEvent obj)
    {
        SwitchMenu(EndScreenMenu);
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe < GameFinishedEvent>(GameFinishedEventToken);
    }
}
