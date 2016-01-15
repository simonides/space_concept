using UnityEngine;
using System.Collections;
using Custom.Base;
public class AnimatedBackgroundDontDestroy : SingletonBase<AnimatedBackgroundDontDestroy> {

	// Use this for initialization
	protected override void Awake () {
        base.Awake(this);
	}
	

    public void DestroyThis()
    {
        Debug.Log("Destroyed Menu Backgroud Animation");
        Destroy(gameObject);
    }

    //destroys this singleton instance if one exist, otherwise does nothing
    public static void TryDestroySingleton()
    {
        Debug.Log("Try to Destroy Menu Background Animation");
        if (AnimatedBackgroundDontDestroy.InstanceExists())
        {
            Destroy(AnimatedBackgroundDontDestroy.GetInstance().gameObject);
        }
    }
}
