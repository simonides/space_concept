using System.Collections;
using TinyMessenger;

public class MenuActiveEvent : GenericTinyMessage<bool>
{

    public MenuActiveEvent(object sender, bool moveMap)
        : base(sender, moveMap) {
    }
}
