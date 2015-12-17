using System.Collections;
using TinyMessenger;

public class PlanetClickedEvent : GenericTinyMessage<Planet>
{

    public PlanetClickedEvent(object sender, Planet planet)
        : base(sender, planet)
    {}
}
