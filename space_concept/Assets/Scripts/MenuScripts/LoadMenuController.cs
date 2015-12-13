using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Custom.Utility;
public class LoadMenuController : MonoBehaviour {
    public UnityEngine.UI.Button buttonObj;
    public RectTransform ListContent;
    static string mapName = "";
    public static bool saveGameAvailable;

    void Awake() {
        CreateSaveGameList();
    }

    public void Button_LoadMap(string f_sceneName)
    {
        Debug.Log("Is xml available?: " + saveGameAvailable);
        if(saveGameAvailable){
            Debug.Log("Try to load XML");
            if (SettingsController.GetInstance().LoadMap(mapName))
            {
                Debug.Log("Loaded XML SaveGame correctly");
                    Button_LoadScene(f_sceneName);
            }
        }
    }

    public void Button_LoadScene(string f_sceneName)
    {
        //Application.LoadLevel(f_sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(f_sceneName);
        Debug.Log("Loading Game scene");
    }
    public void Button_LoadScene(int f_sceneIndex)
    {
        //Application.LoadLevel(f_sceneIndex);
        UnityEngine.SceneManagement.SceneManager.LoadScene(f_sceneIndex);
    }

    public void Button_SelectSaveGame(UnityEngine.UI.Text f_text)
    {
        //DO SOMETHING
        //find the savegame
        Debug.Log("Select Load is called");
        bool isThere = SaveFileSerializer.FileExists("SaveGames", f_text.text+".xml");
        if (isThere == true)
        {
            mapName = f_text.text;
            saveGameAvailable = true;
            SettingsController.GetInstance().mapName = mapName;//set the selected mapname in the settings controller
            Debug.Log("Selected SaveGame: " +mapName);
        }
        else
        {
            saveGameAvailable = false;
        }
       
    }

    private void CreateSaveGameList() {
        List<string> foundFiles = new List<string>();
        UnityEngine.UI.Button newButton; 
         foundFiles = SaveFileSerializer.GetFileNames("SaveGames");
        for (int i = 0; i < foundFiles.Count; i++) {
            newButton = Instantiate(buttonObj);
            newButton.transform.SetParent(ListContent);
            newButton.GetComponent<RectTransform>().transform.localScale = Vector3.one;//without this, the buttons have a bigger scale for undefined reasons
            newButton.GetComponentInChildren<UnityEngine.UI.Text>().text = foundFiles[i];            
        }
    }
}
