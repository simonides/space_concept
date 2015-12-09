
using System.Collections;
using TinyMessenger;

public class ToggleInGameMenuEvent : GenericTinyMessage<bool>
{
    public ToggleInGameMenuEvent(object sender, bool isMenuActive)
        : base(sender, isMenuActive) {
    }
}
