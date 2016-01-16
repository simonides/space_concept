using System.Collections;
using TinyMessenger;

public class TactileBackgroundStateEvent : GenericTinyMessage<bool>
{

    public TactileBackgroundStateEvent(object sender, bool enable)
        : base(sender, enable)
    { }
}