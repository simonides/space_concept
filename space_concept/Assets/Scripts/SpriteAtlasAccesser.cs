using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpriteAtlasAccesser : MonoBehaviour {

    public Sprite [] sprites;

    public Dictionary<string, Sprite> atlasSprites;

    void Awake()
    {
        atlasSprites = new Dictionary<string, Sprite>();
    }


	void Start () {
        Debug.Log("LOADING: planet sprite atlas");
        //Sprite[] sprites = Resources.LoadAll<Sprite>("Planet_Atlas");

        foreach (Sprite s in sprites){
            Debug.Log("ADDING SPRITE TO DICTORNARY: " + s.name);
            atlasSprites.Add(s.name, s);
        }
        int size = sprites.Length;
        for (int i = 0; i < size; i++)
        {
            sprites[i] = null;
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
