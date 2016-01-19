using UnityEngine;
using System.Collections;
using TinyMessenger;

/**
 * Planet Object.
 * Is initialised with a Planet Entity (whose position, name and diameter should not be modified outside afterwards).
 * Contains display properties and is attached to a planet object in the game.
 */
[RequireComponent(typeof(SpriteRenderer))]
public class Planet : MonoBehaviour {

    // ****    CONFIGURATION    **** // 
    public Sprite[] planetSprites;
    public Sprite[] planetFX;
    public SpriteRenderer glow;
    public Material glowMaterial;

    public Sprite[] infoSigns;
    public SpriteRenderer sign;
    public enum SignType { Success, Neutral, Warning, Nothing };

    public float MinGlowScale = 1.2f;
    public float MaxGlowScale = 1.5f;
    public float GlowPulseSpeed = 1.1f;

    // ****  ATTACHED OBJECTS   **** //
    SpriteRenderer spriteRenderer;
    CircleCollider2D spriteCollider;
    Transform glowTransform;

    // ****                     **** //
    public PlanetData planetData { get; private set; }
    private bool isSelected = false;
    public float CurrentGlowScale;  // In percent [0..1]
    public bool GlowIsGrowing;
    private float glowUpscaling = 1f;   // Upscaling factor of the glow depending on the planet size


    private bool shakePlanet = false;
    // ****                     **** //
    AudioController audioCon;

    TinyMessageSubscriptionToken planetUpdateEventSubscriptionToken;
    private TinyMessageSubscriptionToken setPlanetSignToken;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            throw new MissingComponentException("Unable to find SpriteRenderer on Planet. The planet game object should have a sprite renderer for the planet texture.");
        }
        spriteCollider = GetComponent<CircleCollider2D>();
        if (spriteCollider == null) {
            throw new MissingComponentException("Unable to find SpriteCopllider on Planet. The planet game object should have a sprite renderer for the planet texture.");
        }

        glowTransform = this.transform.FindChild("Glow");
        if (glowTransform == null) {
            throw new MissingComponentException("Unable to find glow gameobject on Planet. The planet game object should have a child game object called 'Glow'");
        }
        glow = glowTransform.gameObject.GetComponent<SpriteRenderer>();
        if (glow == null) {
            throw new MissingComponentException("Unable to find glow sprite renderer on Planet. The planet game object should have a child game object called 'Glow and a sprite renderer attached to it.'");
        }

        spriteRenderer.transform.localScale = new Vector3(100, 100, 0);

        audioCon = AudioController.GetInstance();

        CurrentGlowScale = 0;
        GlowIsGrowing = true;
        planetUpdateEventSubscriptionToken = MessageHub.Subscribe<PlanetUpdateEvent>((PlanetUpdateEvent evt) => UpdateGraphicalRepresentation());
        setPlanetSignToken = MessageHub.Subscribe<SetPlanetSignEvent>(SetPlanetSign);
    }

    void OnDestroy() {
        MessageHub.Unsubscribe<PlanetUpdateEvent>(planetUpdateEventSubscriptionToken);
        MessageHub.Unsubscribe<SetPlanetSignEvent>(setPlanetSignToken);
    }


    public void Init(PlanetData planet) {
        planetData = planet;
        this.name = "Planet '" + planet.Name + "'";
        this.transform.position = planet.Position;
       // spriteRenderer = GetComponent<SpriteRenderer>();
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
        spriteCollider.radius = spriteSize.x; 

        //Smaller planets need a larger outline (in comparison with the planet), to let it appear as thick as for big planets:
        glowUpscaling = (planetData.Diameter + 5) / planetData.Diameter;

        spriteSize = glow.sprite.rect.size;
        if (spriteSize.x != spriteSize.y) {
            Debug.LogWarning("The used planet sprite for glow is not rectangular and therefore distorted");
        }
        sign.transform.position = new Vector3(sign.transform.position.x, transform.position.y + planetData.Diameter * 1.6f, transform.position.z);
        UpdateGraphicalRepresentation();
    }
    
    private void UpdateGraphicalRepresentation() {
        glow.material = glowMaterial;
        if(planetData.Owner == null) {  // no owner
            glow.material.color = Color.white;
        } else {
            glow.material.color = planetData.Owner.Color;
        }
    }

    public void setSelected(bool selected) {
        this.isSelected = selected;
    }


    public void SingleTouchClick() {
        Debug.Log("planet clicked ");
        audioCon.PlaySound(AudioController.SoundCodes.PlanetSelection);
        StartCoroutine(ShakePlanet());
        MessageHub.Publish(new PlanetClickedEvent(this, this));
    }

    private IEnumerator ShakePlanet()
    {
        if (!shakePlanet)
        {



            shakePlanet = true;
            Vector3 pos = transform.position;
            float wobbleDistance = planetData.Diameter * 0.5f;
            wobbleDistance = 10;
            Vector3 rightMax = pos + Vector3.right * wobbleDistance;
            Vector3 leftMax = pos + Vector3.left * wobbleDistance;
            float duration = 0.1f;
            float startTime = Time.time;

            //from middle to right
            while (Time.time - startTime < duration)
            {
                transform.position = Vector3.Lerp(pos, rightMax, (Time.time - startTime) / (duration));
                //yield return new WaitForSeconds(0.005f);
                yield return new WaitForEndOfFrame();
            }
            //from right  to left
            //duration = 0.2f;
            startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                transform.position = Vector3.Lerp(rightMax, leftMax, (Time.time - startTime) / (duration));
                //yield return new WaitForSeconds(0.005f);
                yield return new WaitForEndOfFrame();
            }
            //from left  to right
            //duration = 0.2f;
            startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                transform.position = Vector3.Lerp(leftMax, rightMax, (Time.time - startTime) / (duration));
                //yield return new WaitForSeconds(0.005f);
                yield return new WaitForEndOfFrame();
            }
            //from right  to left
            //duration = 0.2f;
            startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                transform.position = Vector3.Lerp(rightMax, leftMax, (Time.time - startTime) / (duration));
                //yield return new WaitForSeconds(0.005f);
                yield return new WaitForEndOfFrame();
            }
            //from left  to middle
            //duration = 0.1f;
            startTime = Time.time;
            while (Time.time - startTime < duration)
            {
                transform.position = Vector3.Lerp(leftMax, pos, (Time.time - startTime) / (duration));
                //yield return new WaitForSeconds(0.005f);
                yield return new WaitForEndOfFrame();
            }
            transform.position = pos;
            shakePlanet = false;
        }
        yield return null;
    }

    public void SetPlanetSign(SetPlanetSignEvent event_)
    {
        if (event_.Dictionary.ContainsKey(planetData))
        {
            EvaluationOutcome s;
            event_.Dictionary.TryGetValue(planetData, out s);
            SetSign(s);
        }
        else
        {
            sign.gameObject.SetActive(false);
        }
    }

    private void SetSign(EvaluationOutcome st)
    {
        switch (st)
        {
            case EvaluationOutcome.Success:
                sign.sprite = infoSigns[0]; 
                sign.gameObject.SetActive(true);
                break;
            case EvaluationOutcome.Neutral:
                sign.sprite = infoSigns[1];
                sign.gameObject.SetActive(true);
                break;
            case EvaluationOutcome.Lost:
                sign.sprite = infoSigns[2];
                sign.gameObject.SetActive(true);
                break;
            default: break;
        }
    }
    public void Update() {
        // play pulse-animation
        float change = Time.deltaTime * GlowPulseSpeed;
        float alpha;

        if (isSelected) {
            if(!GlowIsGrowing) { change = -change; }
            CurrentGlowScale += change;
            alpha = 0.5f + 0.5f * Mathf.Sin(CurrentGlowScale * Mathf.PI);
        } else {
            CurrentGlowScale -= Time.deltaTime * GlowPulseSpeed;
            alpha = 1;
        }
        
        if (CurrentGlowScale > 1) {
            CurrentGlowScale = 1;
            GlowIsGrowing = false;
        } else if (CurrentGlowScale < 0) {
            CurrentGlowScale = 0;
            GlowIsGrowing = true;
        }
        Debug.Assert(CurrentGlowScale >= 0 && CurrentGlowScale <= 1);

        float Scale = (MinGlowScale + (MaxGlowScale - MinGlowScale) * Mathf.Sin(CurrentGlowScale * Mathf.PI)) * glowUpscaling;
        

        glowTransform.localScale = new Vector3(Scale, Scale, 1);
        Color clr = glow.material.color;
        clr.a = alpha;
        glow.material.SetColor("_Color", clr);
    }
}
