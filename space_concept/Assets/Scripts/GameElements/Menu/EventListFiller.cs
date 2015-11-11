using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EventListFiller : MonoBehaviour {

    public GameObject sampleButton;
    public Transform contentPanel;
    public List<Item> itemList;

    // Use this for initialization
    void Start()
    {
        PopulateList();
    }

    private void PopulateList()
    {
        foreach (var item in itemList)
        {
            GameObject newButton = Instantiate(sampleButton) as GameObject;
            EventButton eventBtn = newButton.GetComponent<EventButton>();
            eventBtn.name = item.name;
            eventBtn.icon.sprite = item.icon;
            eventBtn.typeLabel.text = item.type;
            eventBtn.rarityLabel.text = item.rarity;
            eventBtn.championIcon.SetActive(item.isChampion);

            eventBtn.button.onClick = item.thingToDo;
            newButton.transform.SetParent(contentPanel, false);
        }
    }


    // Update is called once per frame
    public void SomethingToDo()
    {
        Debug.Log("test me... print ");
    }
}
