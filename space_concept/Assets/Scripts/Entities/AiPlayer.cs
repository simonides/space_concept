using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class AiPlayer : PlayerData {
    public AiPlayer() {
        IsHumanPlayer = false;
    }

    public abstract void PerformNextMovement();     
}
