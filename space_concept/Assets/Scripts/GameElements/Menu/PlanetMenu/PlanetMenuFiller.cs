using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetMenuFiller : MonoBehaviour
{
    public Text Header;

    public void Fill2B(PlanetData planet)
    {
        Header.text = planet.Name;
        //TODO fill more fields
    }
}
