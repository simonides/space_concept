using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Custom.Utility;
public class LoadMenuController : MonoBehaviour {
    public UnityEngine.UI.Button buttonObj;
    public RectTransform ListContent;
    string mapName = "";
    bool saveGameAvailable = false;

    void Awake() {
        CreateSaveGameList();
    }

    public void Button_LoadMap(string f_sceneName)
    {
        if(saveGameAvailable){
            if (SettingsController.GetInstance().LoadMap(mapName))
            {
                Button_LoadScene(f_sceneName);
            }
        }
    }

    public void Button_LoadScene(string f_sceneName)
    {
        Application.LoadLevel(f_sceneName);
    }
    public void Button_LoadScene(int f_sceneIndex)
    {
        Application.LoadLevel(f_sceneIndex);
    }

    public void Button_SelectSaveGame(UnityEngine.UI.Text f_text)
    {
        //DO SOMETHING
        //find the savegame
        if (SaveFileSerializer.FileExists("SaveGames", f_text.text+".xml"))
        {
            mapName = f_text.text;
            saveGameAvailable = true;
        }
       
    }

    private void CreateSaveGameList() {
        List<string> foundFiles = new List<string>();
        UnityEngine.UI.Button newButton; 
        RectTransform rt;
         foundFiles = SaveFileSerializer.GetFileNames("SaveGames");
        for (int i = 0; i < foundFiles.Count; i++) {
            newButton = Instantiate(buttonObj);
            newButton.transform.SetParent(ListContent);
            rt = newButton.GetComponent<RectTransform>();
            float heightpos = (rt.rect.height / 2) * (-1);
            rt.anchoredPosition = new Vector2(ListContent.rect.width / 2, heightpos - rt.rect.height * i);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ListContent.rect.width);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ListContent.rect.height/15);
            
          //  rt.sizeDelta = new Vector2(ListContent.sizeDelta.x, ListContent.sizeDelta.y/10);
            
        }
    }
}
