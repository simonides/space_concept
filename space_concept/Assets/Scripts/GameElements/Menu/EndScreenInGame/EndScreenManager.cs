using UnityEngine;
using System.Collections;
using System;

public class EndScreenManager : AbstractMenuManager
{

   
    public Menu EndScreenMenu;
    
    void Awake()
    {
        MessageHub.Subscribe<GameFinishedEvent>(GameFinished);

    }

    private void GameFinished(GameFinishedEvent obj)
    {
        SwitchMenu(EndScreenMenu);
    }

    
}
