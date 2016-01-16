using System.Collections;
using TinyMessenger;

public class ToggleNextDayButtonEvent : GenericTinyMessage<bool>
{
    public ToggleNextDayButtonEvent(object sender, bool enable)
         : base(sender, enable)
    {
    }
}
