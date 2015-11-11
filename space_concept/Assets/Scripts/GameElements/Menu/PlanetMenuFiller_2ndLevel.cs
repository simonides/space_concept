using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetMenuFiller_2ndLevel : MonoBehaviour {

    public Text PlanetName;

    public void Fill2B(PlanetData planet)
    {
        PlanetName.text = planet.Name;
        //TODO fill more fields
    }
}
