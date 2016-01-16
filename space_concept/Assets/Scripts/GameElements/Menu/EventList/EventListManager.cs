using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;



public class EventListManager : AbstractMenuManager {

    public Menu eventListMenu;
    private EventListFiller _eventListFiller;
    public List<AttackEvaluation> activeEventlist;
    

    void Awake()
    {
        _eventListFiller = GetComponentInChildren<EventListFiller>();
        MessageHub.Subscribe<ShowEventListEvent>(ShowEventList);
        MessageHub.Subscribe<HideEventListEvent>(HideEventList);
        MessageHub.Subscribe<TroopEvaluationResultEvent>(OnTroopEvalDone);
    }

    private void OnTroopEvalDone(TroopEvaluationResultEvent event_)
    {
        activeEventlist = event_.EvaluationData;
        ShowEventList(null);
    }

    public void ShowEventList(ShowEventListEvent event_)
    {
        _eventListFiller.Fill(activeEventlist);
        SwitchMenu(eventListMenu);
    }

    //public void ShowEventList()

    public void HideEventList(HideEventListEvent event_)
    {
        SwitchMenu(null);
    }
}
