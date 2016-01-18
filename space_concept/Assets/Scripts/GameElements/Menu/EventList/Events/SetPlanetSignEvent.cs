using TinyMessenger;
using System.Collections.Generic;
public class SetPlanetSignEvent : TinyMessageBase
{

    public Dictionary<PlanetData, EvaluationOutcome> Dictionary { get; private set; }
    public SetPlanetSignEvent(object sender, Dictionary<PlanetData, EvaluationOutcome> dict)
        : base(sender)
    {
        Dictionary = dict;
    }
}
