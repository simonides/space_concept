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
    public GameObject FactoryNormalPnl;
    public GameObject FactoryDisableText;

    public Button HangarBtn;
    public Text HangarSizeAndFillsize;
    public Text HangarIncreaseAmount;
    public Text HangarUpgradeCost;
    public Text HangarCostLbl;
    public Image HangarLevel;
    public Image HangarLevelMax;
    public GameObject HangarNormalPnl;
    public GameObject HangarDisableText;

    public Button SendShipsBtn;
    public Text SendShipText;

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
        FactoryLevelMax.sprite = number.GetSpriteForNumber(activePlanet.GetNextFactoryLevel());

        FactoryUpgradeCost.text = "" + activePlanet.GetFactoryUpgradeCosts();
        FactoryIncreaseAmount.text = "+" + activePlanet.GetNextFactoryUpgrade();
        FactoryActualIncreaseSpeed.text = "+" + activePlanet.FactorySpeed;
        
        var isFactoryMaxedOut = (activePlanet.FactoryLevel == activePlanet.MaxFactoryLevel);
        if (isFactoryMaxedOut) {
            Debug.Log("Factory Maxed out");
        }
        FactoryDisableText.SetActive(isFactoryMaxedOut);
        FactoryBtn.interactable = !isFactoryMaxedOut;
        FactoryNormalPnl.SetActive(!isFactoryMaxedOut);


    }

    public void UpgradeHangarUpdate(){

        HangarUpgradeCost.text = "" + activePlanet.GetHangarUpgradeCosts();
        HangarIncreaseAmount.text = "+" + activePlanet.GetNextHangarUpgrade();

        var isHangarMaxedOut = (activePlanet.HangarLevel == activePlanet.MaxHangarLevel);
        if (isHangarMaxedOut){
            Debug.Log("Hangar Maxed out");
        }
        HangarDisableText.SetActive( isHangarMaxedOut);
        HangarBtn.interactable = !isHangarMaxedOut;
        HangarNormalPnl.SetActive(!isHangarMaxedOut);

        HangarLevel.sprite = number.GetSpriteForNumber(activePlanet.HangarLevel);
        HangarLevelMax.sprite = number.GetSpriteForNumber(activePlanet.GetNextHangarLevel());
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
        var planetHasShips = activePlanet.Ships != 0;
        SendShipsBtn.interactable = planetHasShips;
        if (planetHasShips){
            SendShipText.color = Color.white;
        }
        else
        {
            SendShipText.color = new Color32(126,126,126,126);

        }
    }
}
