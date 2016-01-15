using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class AiPlayer {
    PlayerData playerData;

    //seralization needs a default public constructor
    public AiPlayer()
    {

    }
    public AiPlayer(PlayerData playerData) {
        this.playerData = playerData;
        playerData.IsHumanPlayer = false;
    }

    public void PerformNextMovement() {
        Debug.Log("The AI '" + playerData.Name + "' does nothing, because it does not know what to do. This AI is stupid. It has no brain.");
    }
}
