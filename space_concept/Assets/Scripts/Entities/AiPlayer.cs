using UnityEngine;
using System.Collections;

public abstract class AiPlayer : PlayerData {
    public AiPlayer() {
        IsHumanPlayer = false;
    }

    public abstract void PerformNextMovement();     
}
