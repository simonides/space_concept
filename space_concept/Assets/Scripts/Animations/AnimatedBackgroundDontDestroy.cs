using UnityEngine;
using System.Collections;
using Custom.Base;
public class AnimatedBackgroundDontDestroy : SingletonBase<AnimatedBackgroundDontDestroy> {

	// Use this for initialization
	void Awake () {
        base.Awake(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
