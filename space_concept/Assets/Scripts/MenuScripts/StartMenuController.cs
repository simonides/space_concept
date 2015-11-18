using UnityEngine;
using System.Collections;

public class StartMenuController : MonoBehaviour {
    public string fieldNameInput = "";
    public Color playerColor;
    public UnityEngine.UI.Button colorButton;
    public UnityEngine.UI.InputField name;
    void Awake() {
        playerColor = SettingsController.GetInstance().playerFile.Color;
        fieldNameInput = SettingsController.GetInstance().playerFile.Name;

        //set color of button
        UnityEngine.UI.ColorBlock cb = colorButton.colors;
        cb.normalColor = playerColor;
        cb.pressedColor = playerColor;
        cb.highlightedColor = playerColor;
        colorButton.colors = cb;

        //set name
        name.text = fieldNameInput;
    }

    public void Button_LoadScene(string f_sceneName){
        Application.LoadLevel(f_sceneName);
        SaveChanges();//save changes when leaving this menu scene
    }
    public void Button_LoadScene(int f_sceneIndex){
        Application.LoadLevel(f_sceneIndex);
        SaveChanges(); //save changes when leaving this menu scene
    }

    public void TextField_OnEditFinish(UnityEngine.UI.Text f_input) {
        Debug.Log(fieldNameInput);
        SettingsController.GetInstance().playerFile.Name = f_input.text;//wirte text into playerData --- not nice at the moment this way
    }

    public void Button_ChooseColor(UnityEngine.UI.Button f_button) {
        playerColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        UnityEngine.UI.ColorBlock cb = f_button.colors;
        cb.normalColor = playerColor;
        cb.pressedColor = playerColor;
        cb.highlightedColor = playerColor;
        f_button.colors = cb;

        SettingsController.GetInstance().playerFile.Color = playerColor;//wirte color into playerData --- not nice at the moment this way
    }

    private void SaveChanges() {
        SettingsController.GetInstance().SaveData();
    }
}
