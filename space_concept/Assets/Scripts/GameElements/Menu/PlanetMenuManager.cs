using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetMenuManager : MonoBehaviour
{
    public Menu PlanetMenu;
    private Menu _currentMenu;
    private PlanetMenuFiller _planetMenuFiller;

    void Awake()
    {

        _planetMenuFiller = this.GetComponentInChildren<PlanetMenuFiller>();

    }


    // close the current menu
    public void SetPlanetMenuInVisible()
    {
        if (_currentMenu != null){
            _currentMenu.IsOpen = false;
        }
    }

    public void SetPlanetMenuVisible()
    {
        // needed to switch menu
        // if (CurrentMenu != null)
        // {
        //     CurrentMenu.IsOpen = false;
        // }
        // CurrentMenu = menu;
        // CurrentMenu.IsOpen = true;
        _currentMenu = PlanetMenu;
        PlanetMenu.IsOpen = true;
    }

    public void FillPlanetMenu(PlanetData planet)
    {
        _planetMenuFiller.Fill2B(planet);
    }
}
