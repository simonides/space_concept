using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Custom.Utility;
public class LoadMenuController : MonoBehaviour {
    public UnityEngine.UI.Button buttonObj;
    public UnityEngine.UI.Button buttonDelete;
    public RectTransform saveGamePanel;
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
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(f_sceneName);
        Debug.Log("Loading Game scene");
    }
    public void Button_LoadScene(int f_sceneIndex)
    {
        //Application.LoadLevel(f_sceneIndex);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(f_sceneIndex);
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
            Button_LoadMap("Game");
        }
        else
        {
            Debug.LogError("No savegame found.");
            saveGameAvailable = false;
        }
       
    }

    public void Button_DeleteSaveGame(UnityEngine.UI.Button delButton)
    {
        Debug.Log(delButton.name);
        Transform parent = delButton.transform.parent;//the panel
        Debug.Log(parent.name);
        Debug.Log(buttonObj.name);
        UnityEngine.UI.Button namedButton = parent.Find(buttonObj.name).GetComponent<UnityEngine.UI.Button>();//find the button who holds the name of the file to be deleted

        //get string to delete
        string delName = namedButton.GetComponentInChildren<UnityEngine.UI.Text>().text + ".xml";
        mapName = "";
        SettingsController.GetInstance().mapName = "";
        namedButton.Select();
        SaveFileSerializer.DeleteFile("SaveGames", delName);
        
        //Destroy(parent.GetComponent<UnityEngine.UI.HorizontalLayoutGroup>());
        //Destroy(parent.GetComponent<UnityEngine.UI.LayoutElement>());
        //Destroy(parent.GetComponent<UnityEngine.UI.Image>());
        Destroy(parent.gameObject);
    }

    private void CreateSaveGameList() {
        List<string> foundFiles = new List<string>();
        UnityEngine.UI.Button newButton;
        RectTransform newPanel;
         foundFiles = SaveFileSerializer.GetFileNames("SaveGames");
        for (int i = 0; i < foundFiles.Count; i++) {
            //add new panel
            newPanel = Instantiate(saveGamePanel);
            newPanel.transform.SetParent(ListContent);
            newPanel.transform.localScale = Vector3.one;
            //rename the Button correctly
            newButton = newPanel.Find(buttonObj.name).GetComponent<UnityEngine.UI.Button>();
            newButton.GetComponentInChildren<UnityEngine.UI.Text>().text = foundFiles[i];

            //newButton = Instantiate(buttonObj);
            //newButton.transform.SetParent(ListContent);
            //newButton.GetComponent<RectTransform>().transform.localScale = Vector3.one;//without this, the buttons have a bigger scale for undefined reasons
            //newButton.GetComponentInChildren<UnityEngine.UI.Text>().text = foundFiles[i];            
        }
    }
}
