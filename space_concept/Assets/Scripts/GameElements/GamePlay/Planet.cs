﻿using UnityEngine;
using System.Collections;




/**
 * Planet Object.
 * Is initialised with a Planet Entity (whose position, name and diameter should not be modified outside afterwards).
 * Contains display properties and is attached to a planet object in the game.
 */
[RequireComponent(typeof(SpriteRenderer))]
public class Planet : MonoBehaviour {
    public Sprite[] planetSprites;
    public Sprite[] planetFX;
    public SpriteRenderer glow;
    public Material glowMaterial;
    //public Material glowMatNeutral;
    //public Material glowMatOwned;
    //public Material glowMatEnemy;

    //private Material _usualGlow;




    // ****    CONFIGURATION    **** //    

    // ****  ATTACHED OBJECTS   **** //
    SpriteRenderer spriteRenderer;
    CircleCollider2D spriteCollider;

    // ****                     **** //
    public PlanetData planetData { get; private set; }

    // ****                     **** //



    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            throw new MissingComponentException("Unable to find SpriteRenderer on Planet. The planet game object should have a sprite renderer for the planet texture.");
        }
        spriteCollider = GetComponent<CircleCollider2D>();
        if (spriteCollider == null) {
            throw new MissingComponentException("Unable to find SpriteCopllider on Planet. The planet game object should have a sprite renderer for the planet texture.");
        }
    }


    void Start() {
        MessageHub.Subscribe<PlanetUpdateEvent>((PlanetUpdateEvent evt) => UpdateGraphicalRepresentation());
    }


    public void Init(PlanetData planet) {
        planetData = planet;
        this.name = "Planet '" + planet.Name + "'";
        this.transform.position = planet.Position;

        //load planet sprite
        if (planet.TextureName == "") {
            int index = Random.Range(0, planetSprites.Length - 1);
            spriteRenderer.sprite = planetSprites[index];
            planet.TextureName = spriteRenderer.sprite.name;
        } else {
            foreach (Sprite s in planetSprites) {
                if (s.name == planet.TextureName) {
                    spriteRenderer.sprite = s;
                }
            }
        }
        if (planet.TextureFXName == "") {
            glow.sprite = planetFX[0];
            planet.TextureFXName = planetFX[0].name;
        } else {
            foreach (Sprite s in planetFX) {
                if (s.name == planet.TextureFXName) {
                    glow.sprite = s;
                }
            }
        }
        Vector2 spriteSize = spriteRenderer.sprite.rect.size;
        if (spriteSize.x != spriteSize.y) {
            Debug.LogWarning("The used planet sprite is not rectangular and therefore distorted");
        }
        this.transform.localScale = new Vector3(planet.Diameter / spriteSize.x, planet.Diameter / spriteSize.y, 1);
        spriteCollider.radius = spriteSize.x * 0.5f;


        spriteSize = glow.sprite.rect.size;
        if (spriteSize.x != spriteSize.y) {
            Debug.LogWarning("The used planet sprite for glow is not rectangular and therefore distorted");
        }
        glow.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);//new Vector3(planet.Diameter / spriteSize.x, planet.Diameter / spriteSize.y, 1);

        UpdateGraphicalRepresentation();
    }




    private void UpdateGraphicalRepresentation() {
        glow.material = glowMaterial;
        glow.material.color = Color.red;
    }

    public void setSelected(bool selected) {
        //TODO: implement
    }


    public void SingleTouchClick() {
        Debug.Log("planet clicked ");
        MessageHub.Publish(new PlanetClickedEvent(this, this));
    }

    //public Vector2 position {                           // The position of this planet in the cosmos
    //    get { return transform.localPosition; }
    //    set { transform.localPosition = value; }
    //}
}
