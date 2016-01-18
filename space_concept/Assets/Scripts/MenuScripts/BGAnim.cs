using UnityEngine;
using System.Collections;
using Custom.Base;
public class BGAnim : SingletonBase<BGAnim> {
    [Header("Anim sequence, 1, 1, 2, 1, 3")] 
    public GameObject[] backgrounds;

    public GameObject[] movingParts;
    public float speed;
    private float movePoint;
    private float halfSizeOfSprite;
    Vector3 moveingSpeed;
    Vector3 movingPartDistance;
    void Awake(){
        base.Awake(this);
        movingParts = new GameObject[5];
        movingParts[0] = Instantiate(backgrounds[0]);
        movingParts[1] = Instantiate(backgrounds[0]);
        movingParts[2] = Instantiate(backgrounds[1]);
        movingParts[3] = Instantiate(backgrounds[0]);
        movingParts[4] = Instantiate(backgrounds[2]);

        movePoint = transform.position.x - 200;
        halfSizeOfSprite = backgrounds[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x *backgrounds[0].transform.localScale.x;
        movingPartDistance = new Vector3(halfSizeOfSprite, 0, 0);
        moveingSpeed = Vector3.left;

        for (int i = 0; i < 5; i++) { 
            movingParts[i].transform.SetParent(transform);
            movingParts[i].transform.position = transform.position + Vector3.right * (halfSizeOfSprite * i);
            movingParts[i].SetActive(true);
        }
       
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        for(int i = 0 ; i < 5; i++)
        {
            movingParts[i].transform.position += moveingSpeed * speed * Time.deltaTime;
            if (movePoint > movingParts[i].transform.position.x)
            {
                movingParts[i].transform.position = movingParts[(i + 4) % 5].transform.position + movingPartDistance;
            }
        }

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
        if (BGAnim.InstanceExists())
        {
            Destroy(BGAnim.GetInstance().gameObject);
        }
    }
}
