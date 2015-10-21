using UnityEngine;
using System.Collections;




/**
 * Planet Object.
 * Is initialised with a Planet Entity (whose position, name and diameter should not be modified outside afterwards).
 * Contains display properties and is attached to a planet object in the game.
 */
[RequireComponent(typeof(SpriteRenderer))]
public class Planet : MonoBehaviour {
   

    public Sprite planetSprite { get; private set; }    // The image of this planet

    PlanetData planetData;

    public void Init(PlanetData planet) {
        planetData = planet;
    }

    //public Vector2 position {                           // The position of this planet in the cosmos
    //    get { return transform.localPosition; }
    //    set { transform.localPosition = value; }
    //}
}
