using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SpriteRenderer))]
public class Troop : MonoBehaviour, Poolable {
    
    TroopData troopData;

    public void Init(TroopData troop) {
        troopData = troop;
    }

    public void Reset() {
        troopData = null;
    }
    
}
