using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour 
{

    public static UIManager instance;

    private PlanetMenuManager _planetMenuManager;


    public void PlanetClicked(Planet planet)
    {
        _planetMenuManager.ActivePlanet = planet.planetData;
        _planetMenuManager.SwitchToFirstLevel();
    }

    public void HideAllMenus()
    {
        _planetMenuManager.SetPlanetMenuInVisible();
    }

    void Awake()
    {
        instance = this; // TODO this is hazardous!! if the ui manager is ever needed in the awake function nothing can guarantee the order and whether this has been initialized
        _planetMenuManager = this.GetComponent<PlanetMenuManager>();
    }

}
