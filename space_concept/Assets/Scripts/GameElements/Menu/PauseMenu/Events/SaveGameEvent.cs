using System.Collections;
using TinyMessenger;

public class SaveGameEvent : GenericTinyMessage<string>
{
    public SaveGameEvent(object sender, string filename) 
        : base(sender, filename)
    {
    }
}

