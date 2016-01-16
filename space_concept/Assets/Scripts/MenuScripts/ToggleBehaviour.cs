using UnityEngine;
using System.Collections;

public class ToggleBehaviour : MonoBehaviour {
   
    
    private UnityEngine.UI.Toggle myToggle;
    private Animator toggleAnimator;


	// Use this for initialization
	void Awake () {
        myToggle = gameObject.GetComponent<UnityEngine.UI.Toggle>();
        toggleAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ToggleSwitched()
    {

        //slide from left to right
        toggleAnimator.SetBool("isOn", myToggle.isOn);
    }
}
