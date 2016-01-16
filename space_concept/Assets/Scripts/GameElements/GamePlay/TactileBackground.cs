using UnityEngine;
using System.Collections.Generic;

public class TactileBackground : MonoBehaviour {

    // ****    CONFIGURATION    **** //
    public GameObject tactileBackgroundPrefab;
    public float tactileBackgroundWith;

    public float maxAlpha = 0.7f;
    public float fadeSpeed = 2f;
    // ****                     **** //

    //public bool should
    public GameObject backgroundHolder;

    public Rect bounds { get; private set; }     // Size/bounds of the cosmos


    public void Awake() {
        Debug.Assert(tactileBackgroundPrefab != null);
        Debug.Assert(tactileBackgroundWith > 0);
    }
    public void Init(Rect bounds) {
        DestroyChildren();
        this.bounds = bounds;
        SetupTactileBakground();
        
    }

    void DestroyChildren() {
        Destroy(backgroundHolder);
        //var children = new List<GameObject>();
        //foreach (Transform child in transform) {
        //    children.Add(child.gameObject);
        //}
        //children.ForEach(child => Destroy(child));
    }

    void SetupTactileBakground() {
        backgroundHolder = new GameObject();
        backgroundHolder.name = "BackgroundHolder";
        backgroundHolder.transform.SetParent(this.transform);
        Vector2 pos = new Vector2(0,0);
        for (pos.x = -tactileBackgroundWith; pos.x > bounds.xMin- tactileBackgroundWith; pos.x -= tactileBackgroundWith) {
            SetupTactileBakgroundColumn(pos);
        }
        for (pos.x = 0; pos.x < bounds.xMax+ tactileBackgroundWith; pos.x += tactileBackgroundWith) {
            SetupTactileBakgroundColumn(pos);
        }
        
    }
    void SetupTactileBakgroundColumn(Vector2 pos) {
        for (pos.y = -tactileBackgroundWith; pos.y > bounds.yMin-tactileBackgroundWith; pos.y -= tactileBackgroundWith) {
            AddTactileBackgroundSnippet(pos);
        }
        for (pos.y = 0; pos.y < bounds.yMax+ tactileBackgroundWith; pos.y += tactileBackgroundWith) {
            AddTactileBackgroundSnippet(pos);
        }
    }


    void AddTactileBackgroundSnippet(Vector2 position) {
        var bgSnippet = GameObject.Instantiate(tactileBackgroundPrefab) as GameObject;
        bgSnippet.transform.SetParent(backgroundHolder.transform);
        bgSnippet.transform.localPosition = position;
    }
                                                 // Use this for initialization
    void Start () {
        //MessageHub.Subscribe<TactileBackgroundStateEvent>()
	}
	

    void Update() {
        //this.gameObject.SetActive(true);
    }

}
