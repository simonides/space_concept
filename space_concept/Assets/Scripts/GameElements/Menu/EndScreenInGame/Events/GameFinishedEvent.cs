using System.Collections.Generic;
using TinyMessenger;

public class GameFinishedEvent : GenericTinyMessage<WinnerData> {
    public GameFinishedEvent(object sender, WinnerData data)
        : base(sender, data) { }
}