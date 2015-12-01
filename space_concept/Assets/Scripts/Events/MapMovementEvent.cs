using UnityEngine;
using System.Collections;
using TinyMessenger;

public class MapMovementEvent : GenericTinyMessage<bool>
{

    public MapMovementEvent(object sender, bool moveMap)
        : base(sender, moveMap) {
    }
}
