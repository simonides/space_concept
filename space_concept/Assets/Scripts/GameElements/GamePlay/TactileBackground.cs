using UnityEngine;
using System.Collections.Generic;

public class TactileBackground : MonoBehaviour {

    // ****    CONFIGURATION    **** //
    public GameObject tactileBackgroundPrefab;
    public float tactileBackgroundWith;
    public UnityEngine.UI.Text targetPlanetText;

    public float maxAlpha;
    public float fadeSpeed;
    // ****                     **** //

    GameObject backgroundHolder;
    bool shouldBeVisible;
    float currentProgress;

    Rect bounds;     


    public void Awake() {
        Debug.Assert(targetPlanetText != null);
        Debug.Assert(tactileBackgroundPrefab != null);
        Debug.Assert(tactileBackgroundWith > 0);
    }
    public void Init(Rect bounds) {
        shouldBeVisible = false;
        currentProgress = 0;

        DestroyChildren();
        this.bounds = bounds;
        SetupTactileBakground();

    }

    void DestroyChildren() {
        Destroy(backgroundHolder);        
    }

    void SetupTactileBakground() {
        backgroundHolder = new GameObject();
        backgroundHolder.name = "BackgroundHolder";
        backgroundHolder.transform.SetParent(this.transform);
        Vector2 pos = new Vector2(0, 0);
        for (pos.x = -tactileBackgroundWith; pos.x > bounds.xMin - tactileBackgroundWith; pos.x -= tactileBackgroundWith) {
            SetupTactileBakgroundColumn(pos);
        }
        for (pos.x = 0; pos.x < bounds.xMax + tactileBackgroundWith; pos.x += tactileBackgroundWith) {
            SetupTactileBakgroundColumn(pos);
        }

    }
    void SetupTactileBakgroundColumn(Vector2 pos) {
        for (pos.y = -tactileBackgroundWith; pos.y > bounds.yMin - tactileBackgroundWith; pos.y -= tactileBackgroundWith) {
            AddTactileBackgroundSnippet(pos);
        }
        for (pos.y = 0; pos.y < bounds.yMax + tactileBackgroundWith; pos.y += tactileBackgroundWith) {
            AddTactileBackgroundSnippet(pos);
        }
    }


    void AddTactileBackgroundSnippet(Vector2 position) {
        var bgSnippet = GameObject.Instantiate(tactileBackgroundPrefab) as GameObject;
        bgSnippet.transform.SetParent(backgroundHolder.transform);
        bgSnippet.transform.localPosition = position;
    }
    // Use this for initialization
    void Start() {
        MessageHub.Subscribe<TactileBackgroundStateEvent>((TactileBackgroundStateEvent evt) => { this.shouldBeVisible = evt.Content; });
    }



    void Update() {
        float change = Time.deltaTime * fadeSpeed;
        if (!shouldBeVisible) {
            if (currentProgress <= 0) {
                targetPlanetText.gameObject.SetActive(false);
                backgroundHolder.SetActive(false);
                return;
            }
            change = -change;            
        } else {
            targetPlanetText.gameObject.SetActive(true);
            backgroundHolder.SetActive(true);
        }
        currentProgress += change;
        currentProgress = Mathf.Clamp(currentProgress, 0, maxAlpha);

        float alpha = Mathf.Sin(currentProgress * Mathf.PI / 2);
        
        foreach (Transform child in backgroundHolder.transform) {
            var renderer = child.GetComponent<SpriteRenderer>();
            var color = renderer.color;
            color.a = alpha;
            renderer.color = color;
            color = targetPlanetText.color;
            color.a = alpha;
            targetPlanetText.color = color;
        }
    }

}
