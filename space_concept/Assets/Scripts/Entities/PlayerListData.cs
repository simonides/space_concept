using UnityEngine;
using System.Collections.Generic;

public class PlayerListData {

    public PlayerData HumanPlayer { get; private set; }
    public List<AiPlayer> AiPlayers { get; private set; }

    //public PlayerListData() {
    //    HumanPlayer = new PlayerData();
    //    AiPlayers = new List<AiPlayer>();
    //}
    
    public PlayerListData(PlayerData humanPlayer, int aiCount) {
        var aiPlayers = new List<AiPlayer>();
        for(int i=0; i< aiCount; ++i) {
            AiPlayer player = new BasicAI();
            player.Name = PlayerData.GetRandomPlayerName();
            player.Color = Color.blue;  //TODO: predefined list of colors
            aiPlayers.Add(player);
        }
        this.HumanPlayer = humanPlayer;
        this.AiPlayers = aiPlayers;
    }

    public PlayerListData(PlayerData humanPlayer, List<AiPlayer> aiPlayers) {
        this.HumanPlayer = humanPlayer;
        this.AiPlayers = aiPlayers;
    }
}
