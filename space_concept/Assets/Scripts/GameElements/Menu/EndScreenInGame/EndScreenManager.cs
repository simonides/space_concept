using UnityEngine;
using System.Collections;
using System;
using TinyMessenger;

public class EndScreenManager : AbstractMenuManager
{

    public EndScreenFiller filler;

    public Menu EndScreenMenu;
    private TinyMessageSubscriptionToken GameFinishedEventToken;

    void Awake(){
        GameFinishedEventToken = MessageHub.Subscribe<GameFinishedEvent>(GameFinished);
    }

    private void GameFinished(GameFinishedEvent obj)
    {
        filler.Fill(obj.Content);
        SwitchMenu(EndScreenMenu);
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe < GameFinishedEvent>(GameFinishedEventToken);
    }
}
