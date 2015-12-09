using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetMenuManager : MonoBehaviour
{

    public PlanetData ActivePlanet { get; set; }

    public Menu PlanetMenu;
    public Menu PlanetMenu_2ndLevel;
    public Menu InGameOptionsMenu;

    private Menu _currentMenu;
    private PlanetMenuFiller _planetMenuFiller;
    private PlanetMenuFiller_2ndLevel _planetMenuFiller_2ndLevel;

    void Awake()
    {
        _planetMenuFiller = this.GetComponentInChildren<PlanetMenuFiller>();
        _planetMenuFiller_2ndLevel = GetComponentInChildren<PlanetMenuFiller_2ndLevel>();
        MessageHub.Subscribe<ToggleInGameMenuEvent>(MapMovement);
    }

    private void MapMovement(ToggleInGameMenuEvent toggleEvent)
    {
        SwitchMenu(InGameOptionsMenu);
    }

    public void SwitchToFirstLevel()
    {
        _planetMenuFiller.Fill2B(ActivePlanet);
        SwitchMenu(PlanetMenu);
    }

    public void SwitchTo2ndLevel()
    {
        _planetMenuFiller_2ndLevel.Fill2B(ActivePlanet);
        SwitchMenu(PlanetMenu_2ndLevel);
    }

    private void SwitchMenu(Menu menu)
    {
        if (_currentMenu != null)
        {
            _currentMenu.IsOpen = false;
        }
        _currentMenu = menu;
        _currentMenu.IsOpen = true;
    }



    // close the current menu
    public void SetPlanetMenuInVisible()
    {
        if (_currentMenu != null){
            _currentMenu.IsOpen = false;
        }
    }
}
