using UnityEngine;
using System.Collections;

public class StartMenuController : MonoBehaviour {
    public string fieldNameInput = "";
    public Color playerColor;

    public void Button_LoadScene(string f_sceneName){
        Application.LoadLevel(f_sceneName);
    }
    public void Button_LoadScene(int f_sceneIndex){
        Application.LoadLevel(f_sceneIndex);
    }

    public void TextField_OnEditFinish(UnityEngine.UI.Text f_input) {
        Debug.Log(fieldNameInput);
    }

    public void Button_ChooseColor(UnityEngine.UI.Button f_button) {
        playerColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        UnityEngine.UI.ColorBlock cb = f_button.colors;
        cb.normalColor = playerColor;
        cb.pressedColor = playerColor;
        cb.highlightedColor = playerColor;
        f_button.colors = cb;
    }
}
