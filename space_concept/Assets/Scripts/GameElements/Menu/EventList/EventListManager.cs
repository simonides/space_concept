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

    private Dictionary<PlanetData, EvaluationOutcome> planetsWithEvent;
    private Dictionary<PlanetData, EvaluationOutcome> emptyDict;
    private EvaluationOutcome outData;
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
        emptyDict = new Dictionary<PlanetData, EvaluationOutcome>();
    }

    private void OnTroopEvalDone(TroopEvaluationResultEvent event_){
        activeEventlist = event_.EvaluationData;

        //create planet dictionary for this round
        planetsWithEvent = new Dictionary<PlanetData, EvaluationOutcome>();
        foreach(AttackEvaluation ae in activeEventlist){

            if (!planetsWithEvent.ContainsKey(ae.Planet))
            {
                if (ae.Type != EvaluationType.AttackViewer)
                {
                    planetsWithEvent.Add(ae.Planet, ae.Outcome);
                }
                else
                {
                    if (SettingsController.GetInstance().dataFile.fogDist == 0)
                    {
                        planetsWithEvent.Add(ae.Planet, ae.Outcome);
                    }
                }
            }
            else
            {
                planetsWithEvent.TryGetValue(ae.Planet, out outData);
                if (ae.Outcome == EvaluationOutcome.Lost && outData != EvaluationOutcome.Lost)
                {
                    planetsWithEvent[ae.Planet] = ae.Outcome;
                }
                else if(ae.Outcome == EvaluationOutcome.Success && outData == EvaluationOutcome.Neutral){
                    planetsWithEvent[ae.Planet] = ae.Outcome;
                }
            }
        }
        
        MessageHub.Publish(new SetPlanetSignEvent(this, planetsWithEvent));
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
