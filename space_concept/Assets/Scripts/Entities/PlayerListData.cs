using UnityEngine;
using System.Collections.Generic;

public class PlayerListData {

    public PlayerData HumanPlayer { get; private set; }
    public List<AiPlayer> AiPlayers { get; private set; }

    public PlayerListData() {
        HumanPlayer = new PlayerData();
        AiPlayers = new List<AiPlayer>();
    }


    public PlayerListData(PlayerData humanPlayer, int aiCount) {
        Debug.Log("Placing all Players on the map...");
        // TODO: implement
    }

    public PlayerListData(PlayerData humanPlayer, List<AiPlayer> aiPlayers) {
        Debug.Log("Placing all Players on the map...");
        this.AiPlayers = aiPlayers;
    }
}
