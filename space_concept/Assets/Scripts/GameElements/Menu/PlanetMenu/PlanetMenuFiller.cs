using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TinyMessenger;

public class PlanetMenuFiller : MonoBehaviour
{
    public Text Header;
    public Text FactoryActualIncreaseSpeed;
    public Text FactoryIncreaseAmount;
    public Text FactoryUpgradeCost;

    public Text HangarSizeAndFillsize;
    public Text HangarIncreaseAmount;
    public Text HangarUpgradeCost;

    PlanetData activePlanet;

    private TinyMessageSubscriptionToken UpgradeFactoryEventToken;
    private TinyMessageSubscriptionToken UpgradeHangarEventToken;
    private TinyMessageSubscriptionToken NextDayEventToken;

    void Awake()
    {
        Debug.Assert(UpgradeFactoryEventToken == null 
            && UpgradeHangarEventToken == null 
            && NextDayEventToken == null);
        UpgradeFactoryEventToken = MessageHub.Subscribe<UpgradeFactoryEvent>(UpgradeFactory);
        UpgradeHangarEventToken = MessageHub.Subscribe<UpgradeHangarEvent>(UpgradeHangar);
        NextDayEventToken = MessageHub.Subscribe<NextDayEvent>(NextDay);
    }

    private void NextDay(NextDayEvent obj)
    {
        if (activePlanet == null /*|| HangarSizeAndFillsize == null*/) { return; }
        UpgradeHangarUpdate();
        UpgradeFactoryUpdate();
    }

    private void UpgradeHangar(UpgradeHangarEvent event_){
        Debug.Log("Upgrade Hangar.");
        if (activePlanet.UpgradeHangar()){
            UpgradeHangarUpdate();
            UpgradeFactoryUpdate();
        }
    }

    private void UpgradeFactory(UpgradeFactoryEvent event_){
        Debug.Log("Upgrade Factory.");
        if (activePlanet.UpgradeFactory()){
            UpgradeFactoryUpdate();
            UpgradeHangarUpdate();
        }
    }

    public void UpdateInfo(PlanetData planet){
        activePlanet = planet;
        Header.text = planet.Name;
        UpgradeFactoryUpdate();
        UpgradeHangarUpdate();
    }

    public void UpgradeFactoryUpdate(){
        FactoryActualIncreaseSpeed.text = "+" + activePlanet.FactorySpeed;
        FactoryUpgradeCost.text = "" + activePlanet.GetFactoryUpgradeCosts();
        FactoryIncreaseAmount.text = "+" + activePlanet.GetNextFactoryUpgrade();
    }

    public void UpgradeHangarUpdate(){
        HangarSizeAndFillsize.text = activePlanet.Ships + "/" + activePlanet.HangarSize;
        HangarUpgradeCost.text = "" + activePlanet.GetHangarUpgradeCosts();
        HangarIncreaseAmount.text = "" + activePlanet.GetNextHangarUpgrade();
    }

    void OnDestroy()
    {
        MessageHub.Unsubscribe<UpgradeFactoryEvent>( UpgradeFactoryEventToken);
        MessageHub.Unsubscribe<UpgradeHangarEvent>( UpgradeHangarEventToken);
        MessageHub.Unsubscribe<NextDayEvent>(NextDayEventToken);
    }
}
