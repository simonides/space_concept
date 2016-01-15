using UnityEngine;
using System.Collections;

public class StartMenuController : MonoBehaviour {
    public string fieldNameInput = "";
    public Color playerColor;
    public UnityEngine.UI.Button colorButton;
    public UnityEngine.UI.InputField playerName;
    public UnityEngine.UI.Text kiCount;
    public UnityEngine.RectTransform colorPickerPanel;
    private bool loadingScene;
    void Start() {
        playerColor = SettingsController.GetInstance().playerFile.Color;
        fieldNameInput = SettingsController.GetInstance().playerFile.Name;
        SettingsController.GetInstance().loadMap = false; //set it false when entering the startMenu
        //set color of button
        Debug.Log("Loaded Saved Player Color: " + playerColor.ToString());
        UnityEngine.UI.ColorBlock cb = colorButton.colors;
        cb.normalColor = playerColor;
        cb.pressedColor = playerColor;
        cb.highlightedColor = playerColor;
        colorButton.colors = cb;

        //set name
        playerName.text = fieldNameInput;

        loadingScene = false;
    }

    public void Button_LoadScene(string f_sceneName){
       
        if (!loadingScene)
        {
            loadingScene = true;
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(f_sceneName);
            SaveChanges();//save changes when leaving this menu scene
        }
    }
    public void Button_LoadScene(int f_sceneIndex){
   
        if (!loadingScene)
        {
            loadingScene = true;
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(f_sceneIndex);
            SaveChanges(); //save changes when leaving this menu scene
        }
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
    public void Button_OpenColorPicker(UnityEngine.UI.Button f_button)
    {
        Debug.Log("Open Color Picker");
        colorPickerPanel.gameObject.SetActive(true);
        f_button.gameObject.SetActive(false);
    }

    public void Button_PicColor(UnityEngine.UI.Button f_button)
    {
        Debug.Log("Picking Color");
        UnityEngine.UI.ColorBlock cb = f_button.colors;
        playerColor = cb.normalColor;
        Debug.Log("Picked: " + cb.normalColor.ToString());
       
        cb = colorButton.colors;
        cb.normalColor = playerColor;
        cb.pressedColor = playerColor;
        cb.highlightedColor = playerColor;
        Debug.Log("New Player Color: "+playerColor.ToString());
        colorButton.colors = cb;

        colorButton.gameObject.SetActive(true);
        colorPickerPanel.gameObject.SetActive(false);
        SettingsController.GetInstance().playerFile.Color = playerColor;
    }

    public void Slider_OnChange(UnityEngine.UI.Slider slider)
    {
        kiCount.text = slider.value.ToString();
    }
    public void Slider_OnChange(int count)
    {
        kiCount.text = count.ToString();
        
    }
    private void SaveChanges() {
        SettingsController.GetInstance().SaveData();
    }
}
