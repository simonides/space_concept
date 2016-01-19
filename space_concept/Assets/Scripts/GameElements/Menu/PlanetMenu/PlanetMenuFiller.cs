using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TinyMessenger;

public class PlanetMenuFiller : MonoBehaviour
{
    public Number number;

    public Button FactoryBtn;
    public Text Header;
    public Text FactoryActualIncreaseSpeed;
    public Text FactoryIncreaseAmount;
    public Text FactoryUpgradeCost;
    public Text FactoryCostLbl;
    public Image FactoryLevel;
    public Image FactoryLevelMax;

    public Button HangarBtn;
    public Text HangarSizeAndFillsize;
    public Text HangarIncreaseAmount;
    public Text HangarUpgradeCost;
    public Text HangarCostLbl;
    public Image HangarLevel;
    public Image HangarLevelMax;

    public Button SendShipsBtn;

    private PlanetData activePlanet;

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
        UpdateSendShipBtn();

    }

    private void UpgradeHangar(UpgradeHangarEvent event_){
        Debug.Log("Upgrade Hangar.");
        if (activePlanet.UpgradeHangar()){
            UpgradeHangarUpdate();
            UpgradeFactoryUpdate();
        }
        UpdateSendShipBtn();

    }

    private void UpgradeFactory(UpgradeFactoryEvent event_){
        Debug.Log("Upgrade Factory.");
        if (activePlanet.UpgradeFactory()){
            UpgradeFactoryUpdate();
            UpgradeHangarUpdate();
        }
        UpdateSendShipBtn();
    }

    public void UpdateInfo(PlanetData planet){
        activePlanet = planet;
        Header.text = planet.Name;
        UpgradeFactoryUpdate();
        UpgradeHangarUpdate();
        UpdateSendShipBtn();

    }

    public void UpgradeFactoryUpdate(){
        FactoryLevel.sprite = number.GetSpriteForNumber(activePlanet.FactoryLevel);
        FactoryLevelMax.sprite = number.GetSpriteForNumber(activePlanet.MaxFactoryLevel);
        if(activePlanet.FactoryLevel == activePlanet.MaxFactoryLevel){
            Debug.Log("Factory Maxed out");

            FactoryUpgradeCost.text = "maxed Out";
            FactoryIncreaseAmount.text = "";
            FactoryCostLbl.enabled = false;
            FactoryBtn.enabled = false;
        }else{
            FactoryUpgradeCost.text = "" + activePlanet.GetFactoryUpgradeCosts();
            FactoryIncreaseAmount.text = "+" + activePlanet.GetNextFactoryUpgrade();
            FactoryCostLbl.enabled = true;
            FactoryBtn.enabled = true;
        }
        FactoryActualIncreaseSpeed.text = "+" + activePlanet.FactorySpeed;

    }

    public void UpgradeHangarUpdate(){
        if (activePlanet.HangarLevel == activePlanet.MaxHangarLevel)
        {
            Debug.Log("Hangar Maxed out");

            HangarUpgradeCost.text = "maxed Out";
            HangarIncreaseAmount.text = "";
            HangarCostLbl.enabled = false;
            HangarBtn.enabled = false;
        }
        else
        {
            HangarUpgradeCost.text = "" + activePlanet.GetHangarUpgradeCosts();
            HangarIncreaseAmount.text = "+" + activePlanet.GetNextHangarUpgrade();
            HangarCostLbl.enabled = true;
            HangarBtn.enabled = true;

        }
        HangarLevel.sprite = number.GetSpriteForNumber(activePlanet.HangarLevel);
        HangarLevelMax.sprite = number.GetSpriteForNumber(activePlanet.MaxHangarLevel);
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

    private void UpdateSendShipBtn(){
        SendShipsBtn.enabled = activePlanet.Ships != 0;
    }
}
