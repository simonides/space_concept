using UnityEngine;
using System.Collections.Generic;

public class PlayerManager: MonoBehaviour {

    public PlayerListData PlayerListData { get; private set; }

    void Start() {
        InitEventSubscriptions();
    }

    public void Init(PlayerListData playerListData) {
        this.PlayerListData = playerListData;       
    }

    

    void InitEventSubscriptions() {
        MessageHub.Subscribe<NextDayEvent>((NextDayEvent evt) => { PerformAiMovements(); });
    }
    
    void PerformAiMovements() {
        Debug.Log("Perfoming AI movements...");
        var aiPlayers = PlayerListData.AiPlayers;
        foreach (AiPlayer ai in aiPlayers) {
            ai.PerformNextMovement();
        }
    }
}
