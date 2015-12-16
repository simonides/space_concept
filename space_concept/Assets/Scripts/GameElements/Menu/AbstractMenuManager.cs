using UnityEngine;

public class AbstractMenuManager : MonoBehaviour {

    private Menu _currentMenu;

    public void SwitchMenu(Menu menu)
    {
        Debug.Log("Enable World interaction");
        MessageHub.Publish(new MenuActiveEvent(this, true));
        if (_currentMenu != null)
        {
            _currentMenu.IsOpen = false;
        }
        _currentMenu = menu;
        if (_currentMenu != null)
        {
            _currentMenu.IsOpen = true;
        }
        else
        {
            Debug.Log("Disable World interaction");
            MessageHub.Publish(new MenuActiveEvent(this, false));
        }
    }
}
