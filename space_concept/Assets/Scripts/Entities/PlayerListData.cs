using UnityEngine;
using System.Collections.Generic;

public class PlayerListData {

    public PlayerData HumanPlayer { get; private set; }
    public List<AiPlayer> AiPlayers { get; private set; }

    //public PlayerListData() {
    //    HumanPlayer = new PlayerData();
    //    AiPlayers = new List<AiPlayer>();
    //}
    
    public PlayerListData(PlayerData humanPlayer, List<AiPlayer> aiPlayers) {
        this.HumanPlayer = humanPlayer;
        this.AiPlayers = aiPlayers;
    }
}
