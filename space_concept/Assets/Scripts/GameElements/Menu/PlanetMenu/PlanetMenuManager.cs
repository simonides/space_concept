using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetMenuManager : AbstractMenuManager
{

    public PlanetData ActivePlanet { get; set; }

    public Menu PlanetMenu;
    public Menu PlanetMenu_2ndLevel;
    public Menu InGameOptionsMenu;

    private PlanetMenuFiller _planetMenuFiller;
    private PlanetMenuFiller_2ndLevel _planetMenuFiller_2ndLevel;

    void Awake()
    {
        _planetMenuFiller = GetComponentInChildren<PlanetMenuFiller>();
        if (_planetMenuFiller == null)
        {
            throw new MissingComponentException("Unable to find PlanetMenuFiller.");
        }
        _planetMenuFiller_2ndLevel = GetComponentInChildren<PlanetMenuFiller_2ndLevel>();
        if (_planetMenuFiller_2ndLevel == null)
        {
            throw new MissingComponentException("Unable to find _planetMenuFiller_2ndLevel.");
        }
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


    // close the current menu
    public void SetPlanetMenuInVisible()
    {
        SwitchMenu(null);
    }
}
