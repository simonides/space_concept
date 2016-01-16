using System.Collections;
using TinyMessenger;

public class ClickedOnForeignPlanetEvent : GenericTinyMessage<Planet> { 

   public ClickedOnForeignPlanetEvent(object sender, Planet planet) 
        : base(sender, planet)
    {
    }
}
