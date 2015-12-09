using System.Collections;
using TinyMessenger;

public class SaveGameEvent : ITinyMessage {

    public object Sender { get; private set; }

    public string Filename{ get; private set; }

    public SaveGameEvent(object sender, string filename)
    {
        Sender = sender;
        Filename = filename;
    }
}

