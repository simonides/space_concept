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
    public List<AttackEvaluation> itemList;

    public Text NoItemsInListText;
    private int lastRandom = -1;


    public Transform contentPanel;


    private string[] itemsEmptyTexts =
    {
    "Nothing happend. Oh, wait. Something did happen. Actually Simon dropped his bass.",
    "Nothing happened. Except: A bag of rice fell.",
    "Yo, what's up bro? Nothin' happened today. Send some ships, yo!",
    "Wazzaaapp! Nothing. Not a single ship arrived.",
    "Heeellooo? Is there anyone? It's so boring today, nothin's happenin.",
    "Nothing happened.The rooster crowed and noone heard it.",
    "Your enemy wanted to send some ships today. He forgot, so nothing happened.",
    "If you don't send some ships, nothing will ever happen... Nice."
    };


    void Awake()
    {
        pooledGameObjectHolder = GameObject.Find("PooledGameObjects");
        if (pooledGameObjectHolder == null)
        {
            throw new MissingComponentException("Unable to find PooledGameObjects There should be an empty game object that holds the pooled data.");
        }
    }


    // Use this for initialization
    void Start()
    {
        eventButtonPool = new PoolAllocator<GameObject>(
            () =>
            {
                GameObject gObj = GameObject.Instantiate(sampleButton) as GameObject;
                return gObj;
            },
            (GameObject gObj) =>
            {
                gObj.SetActive(false);
                gObj.name = "Pooled EventButton";
                gObj.transform.SetParent(pooledGameObjectHolder.transform, false);
            }
        );
    }

    private void PopulateList()
    {
        SetNothingHappenedText();
       

        //foreach (var item in itemList)
        //{
        //    GameObject newButton = eventButtonPool.Get();
        //    newButton.SetActive(true);
        //    EventButton eventBtn = newButton.GetComponent<EventButton>();
        //    if (eventBtn == null)
        //    {
        //        Debug.Log("eventbutton was null");
        //        newButton.transform.SetParent(contentPanel, false);
        //        continue;
        //    }
        //    //eventBtn.name = item.planetName;
        //    //eventBtn.icon.sprite = item.icon;
        //    //eventBtn.typeLabel.text = item.planetName;
        //    //eventBtn.rarityLabel.text = item.rarity;
        //    //eventBtn.championIcon.SetActive(item.isChampion);

        //    //eventBtn.button.onClick = item.thingToDo;
        //    //eventBtn.gameObject.transform.parent.transform.SetParent(contentPanel, false);
        //    newButton.transform.SetParent(contentPanel, false);
        //    //newButton.transform.DetachChildren()
        //}
    }

    public void Fill(List<AttackEvaluation> events)
    {
        itemList = events;
        PopulateList();
        // delete gameobjects
        // clear old list 
        // set new list 
        // generate new buttons

    }

    private void SetNothingHappenedText()
    {
        if (itemList == null || itemList.Count == 0) {
            int index = Random.Range(0, itemsEmptyTexts.GetLength(0) - 1);
            if (lastRandom == index)
            {
                index = Random.Range(0, itemsEmptyTexts.GetLength(0) - 1);
            }
            lastRandom = index;
            NoItemsInListText.text = itemsEmptyTexts[index];
        }
        else
        {
            NoItemsInListText.text = "";
        }

    }
}
