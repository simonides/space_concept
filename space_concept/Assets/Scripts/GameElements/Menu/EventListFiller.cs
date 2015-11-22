using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventListFiller : MonoBehaviour
{
    // ****    CONFIGURATION    **** //

    // ****  ATTACHED OBJECTS   **** //
    GameObject pooledGameObjectHolder;

    // ****     ALLOCATORS      **** //
    PoolAllocator<GameObject> eventButtonPool;

    // ****                     **** //
    

    public EventButton prefab;

    public GameObject sampleButton;
    public List<PlanetEvent> itemList;


    public Transform contentPanel;


    void Awake() {
        pooledGameObjectHolder = GameObject.Find("PooledGameObjects");
        if (pooledGameObjectHolder == null) {
            throw new MissingComponentException("Unable to find PooledGameObjects There should be an empty game object that holds the pooled data.");
        }
    }


    // Use this for initialization
    void Start() {
        eventButtonPool = new PoolAllocator<GameObject>(
            () => {
                GameObject gObj = GameObject.Instantiate(sampleButton) as GameObject;
                return gObj;
            },
            (GameObject gObj) => {
                gObj.SetActive(false);
                gObj.name = "Pooled EventButton";
                gObj.transform.SetParent(pooledGameObjectHolder.transform, false);
            }
        );
    }

    private void PopulateList()
    {

        foreach (var item in itemList)
        {
            GameObject newButton = eventButtonPool.Get();
            newButton.SetActive(true);
            //newButton.transform.SetParent(null);
            //GameObject newButton = Instantiate(sampleButton) as GameObject;
            //EventButton eventBtn = newButton.GetComponentInChildren<EventButton>();
            EventButton eventBtn = newButton.GetComponent<EventButton>();
            if (eventBtn == null)
            {
                Debug.Log("eventbutton was null");
                newButton.transform.SetParent(contentPanel, false);
                continue;
            }
            eventBtn.name = item.planetName;
            //eventBtn.icon.sprite = item.icon;
            eventBtn.typeLabel.text = item.planetName;
            //eventBtn.rarityLabel.text = item.rarity;
            //eventBtn.championIcon.SetActive(item.isChampion);

            //eventBtn.button.onClick = item.thingToDo;
            //eventBtn.gameObject.transform.parent.transform.SetParent(contentPanel, false);
            newButton.transform.SetParent(contentPanel, false);
            //newButton.transform.DetachChildren()
        }
    }

    public void Fill(List<PlanetEvent> events)
    {
        itemList = events;
        PopulateList();
        // delete gameobjects
        // clear old list 
        // set new list 
        // generate new buttons

    }

    public void SomethingToDo()
    {
        Debug.Log("test me... print ");
    }
}
