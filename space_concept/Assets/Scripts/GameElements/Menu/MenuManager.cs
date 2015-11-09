using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public static MenuManager menuManager;
    public Menu PlanetMenu;
    public Menu CurrentMenu;

    public void Start()
    {
        menuManager = this;
        ShowMenu(CurrentMenu);
    }

    public void ShowMenu(Menu menu)
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.IsOpen = false;
        }
        CurrentMenu = menu;
        CurrentMenu.IsOpen = true;
    }
}
