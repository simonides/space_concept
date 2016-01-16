using System.Collections;
using TinyMessenger;

public struct WinnerData
{
    string name;
}
public class GameFinishedEvent : GenericTinyMessage<WinnerData>
{
    public GameFinishedEvent(object sender, WinnerData data)
        : base(sender, data)
    { }
}