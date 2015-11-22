using UnityEngine;
using System.Collections;




/**
 * Planet Object.
 * Is initialised with a Planet Entity (whose position, name and diameter should not be modified outside afterwards).
 * Contains display properties and is attached to a planet object in the game.
 */
[RequireComponent(typeof(SpriteRenderer))]
public class Planet : MonoBehaviour
{

    // ****    CONFIGURATION    **** //    

    // ****  ATTACHED OBJECTS   **** //
    SpriteRenderer spriteRenderer;

    // ****                     **** //
    public PlanetData planetData { get; private set; }

    // ****                     **** //



    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            throw new MissingComponentException("Unable to find SpriteRenderer on Planet. The planet game object should have a sprite renderer for the planet texture.");
        }
    }


    public void Init(PlanetData planet)
    {
        planetData = planet;
        this.name = "Planet '" + planet.Name + "'";
        this.transform.position = planet.Position;

        Vector2 spriteSize = spriteRenderer.sprite.rect.size;
        if (spriteSize.x != spriteSize.y)
        {
            Debug.LogWarning("The used planet sprite is not rectangular and therefore distorted");
        }
        this.transform.localScale = new Vector3(planet.Diameter / spriteSize.x, planet.Diameter / spriteSize.y, 1);
        //TODO: set background sprite of planet here
    }

    public void SingleTouchClick()
    {
        Debug.Log("planet clicked ");
        UIManager.instance.PlanetClicked(this);
    }

    //public Vector2 position {                           // The position of this planet in the cosmos
    //    get { return transform.localPosition; }
    //    set { transform.localPosition = value; }
    //}
}
