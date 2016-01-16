using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpriteAtlasAccesser : MonoBehaviour {

    public Sprite atlas;

    public Dictionary<string, Sprite> atlasSprites;

    void Awake()
    {
        atlasSprites = new Dictionary<string, Sprite>();
    }


	void Start () {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Planet_Atlas");
        //Sprite[] sprites = atlas;

        foreach (Sprite s in sprites){
            atlasSprites.Add(s.name, s);
        }
	}


    public Sprite GetSprite(string name)
    {
        Sprite s = null;
        atlasSprites.TryGetValue(name, out s);
        return s;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
