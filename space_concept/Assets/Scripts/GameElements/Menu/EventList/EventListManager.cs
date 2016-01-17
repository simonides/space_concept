using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using TinyMessenger;


public class EventListManager : AbstractMenuManager {

    public Menu eventListMenu;
    private EventListFiller _eventListFiller;
    public List<AttackEvaluation> activeEventlist;

    private TinyMessageSubscriptionToken ShowEventListEventToken;
    private TinyMessageSubscriptionToken HideEventListEventToken;
    private TinyMessageSubscriptionToken TroopEvaluationResultEventToken;

    void Awake(){
        _eventListFiller = GetComponentInChildren<EventListFiller>();
        Debug.Assert(ShowEventListEventToken == null && 
            HideEventListEventToken == null && 
            TroopEvaluationResultEventToken == null);
        ShowEventListEventToken = MessageHub.Subscribe<ShowEventListEvent>(ShowEventList);
        HideEventListEventToken = MessageHub.Subscribe<HideEventListEvent>(HideEventList);
        TroopEvaluationResultEventToken = MessageHub.Subscribe<TroopEvaluationResultEvent>(OnTroopEvalDone);
    }

    private void OnTroopEvalDone(TroopEvaluationResultEvent event_){
        activeEventlist = event_.EvaluationData;
        ShowEventList(null);
    }

    public void ShowEventList(ShowEventListEvent event_)
    {
        _eventListFiller.Fill(activeEventlist);
        SwitchMenu(eventListMenu);
    }

    public void HideEventList(HideEventListEvent event_)
    {
        SwitchMenu(null);
    }

    void OnDestroy(){
        MessageHub.Unsubscribe <ShowEventListEvent>(ShowEventListEventToken);
        MessageHub.Unsubscribe <HideEventListEvent>(HideEventListEventToken);
        MessageHub.Unsubscribe <TroopEvaluationResultEvent>(TroopEvaluationResultEventToken);
    }
}
