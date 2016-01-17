using UnityEngine;
using System.Collections.Generic;
using TinyMessenger;

public class PlayerManager: MonoBehaviour {

    public PlayerListData PlayerListData { get; private set; }
    private TinyMessageSubscriptionToken NextDayEventToken;

    void Start() {
        InitEventSubscriptions();
    }

    public void Init(PlayerListData playerListData) {
        this.PlayerListData = playerListData;       
    }

    

    void InitEventSubscriptions() {
        Debug.Assert(NextDayEventToken == null);
        NextDayEventToken = MessageHub.Subscribe<NextDayEvent>((NextDayEvent evt) => { PerformAiMovements(); });
    }
    
    void PerformAiMovements() {
        //Debug.Log("Perfoming AI movements...");
        var aiPlayers = PlayerListData.AiPlayers;
        foreach (AiPlayer ai in aiPlayers) {
            ai.PerformNextMovement();
        }
    }

    void OnDestroy(){
        MessageHub.Unsubscribe<NextDayEvent>(NextDayEventToken);
    }
}
