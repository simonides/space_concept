using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventListFiller : MonoBehaviour
{

    public EventButton prefab;

    public GameObject sampleButton;
    public List<PlanetEvent> itemList;

    PoolAllocator<GameObject> eventButtonPool;

    public Transform contentPanel;
    public Transform objectPool;


    // Use this for initialization
    void Start() {
        eventButtonPool = new PoolAllocator<GameObject>(
        () =>
        {
            GameObject gObj =
                GameObject.Instantiate(sampleButton) as GameObject;
            gObj.SetActive(false);
            gObj.transform.SetParent(objectPool, false);
           return gObj;
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
    // Update is called once per frame
    public void SomethingToDo()
    {
        Debug.Log("test me... print ");
    }
}
