using UnityEngine;
using System.Collections.Generic;

public class PlayerManager: MonoBehaviour {

    public PlayerData HumanPlayer { get; private set; }
    public List<AiPlayer> AiPlayers { get; private set; }

    void Start() {
        AiPlayers = new List<AiPlayer>();
        InitEventSubscriptions();
    }


    public void InitMapWithPlayers(int playerCount) {

    }


    void InitEventSubscriptions() {
        MessageHub.Subscribe<NextDayEvent>((NextDayEvent evt) => { PerformAiMovements(); });
    }
    
}
