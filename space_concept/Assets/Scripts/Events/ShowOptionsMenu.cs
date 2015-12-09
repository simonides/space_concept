using System.Collections;
using TinyMessenger;

public class ShowOptionsMenu : GenericTinyMessage<bool>
{

    public ShowOptionsMenu(object sender, bool moveMap)
        : base(sender, moveMap) {
    }
}
