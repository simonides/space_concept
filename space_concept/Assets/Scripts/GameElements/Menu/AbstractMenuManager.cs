using UnityEngine;

public class AbstractMenuManager : MonoBehaviour {

    private Menu _currentMenu;

    public void SwitchMenu(Menu menu)
    {

        if (_currentMenu != null)
        {
            _currentMenu.IsOpen = false;
        }
        _currentMenu = menu;
        if (_currentMenu != null)
        {
            _currentMenu.IsOpen = true;
        }
    }
}
