using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerListData {

    public PlayerData HumanPlayer { get; private set; }
    public List<AiPlayer> AiPlayers { get; private set; }

    //public PlayerListData() {
    //    HumanPlayer = new PlayerData();
    //    AiPlayers = new List<AiPlayer>();
    //}

    //seralization needs an empty constructor
    public PlayerListData()
    {

    }
    public PlayerListData(PlayerData humanPlayer, List<AiPlayer> aiPlayers) {
        this.HumanPlayer = humanPlayer;
        this.AiPlayers = aiPlayers;
    }
}
