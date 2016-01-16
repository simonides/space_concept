using UnityEngine;
using System.Collections;

public class WinnerData {
    public int day { get; private set; }
    public PlayerListData playerList { get; private set; }

    public WinnerData(int currentDay, PlayerListData playerList) {
        this.day = currentDay;
        this.playerList = playerList;
    }

    public bool DidHumanPlayerSurvive() {
        return playerList.HumanPlayer.GetNumberOfOwnedPlanets() > 0;
    }
}
