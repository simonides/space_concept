using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

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

    void Awake()
    {
        MessageHub.Subscribe<UpgradeFactoryEvent>(UpgradeFactory);
        MessageHub.Subscribe<UpgradeHangarEvent>(UpgradeHangar);
        MessageHub.Subscribe<NextDayEvent>(NextDay);
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
        FactoryIncreaseAmount.text = "" + activePlanet.GetNextFactoryUpgrade();
    }

    public void UpgradeHangarUpdate(){
        HangarSizeAndFillsize.text = activePlanet.Ships + "/" + activePlanet.HangarSize;
        HangarUpgradeCost.text = "" + activePlanet.GetHangarUpgradeCosts();
        HangarIncreaseAmount.text = "" + activePlanet.GetNextHangarUpgrade();
    }


}
