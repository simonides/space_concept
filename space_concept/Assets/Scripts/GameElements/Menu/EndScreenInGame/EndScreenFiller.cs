using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class EndScreenFiller : MonoBehaviour
{

    public Text Line1;
    public Text Line2;
    public Text Line3;
    public Text Line4;


    public void Fill(WinnerData content)
    {
        string lifeform = (content.playerList.AiPlayers.Count == 1 
            ? content.playerList.AiPlayers.Count +  " lifeform" 
            : content.playerList.AiPlayers.Count + " lifeforms");

        if (content.DidHumanPlayerSurvive()) {
            // domination
            // you defated
            // xx lifeforms
            Line1.text = "Domination";
            Line2.text = "You defeated";
            Line3.text = lifeform;
            Line4.text = "";
        }else{
            // extinction
            // you couldn't 
            // compete with
            // xx lifeforms
            Line1.text = "Extinction";
            Line2.text = "You couldn't";
            Line3.text = "compete with";
            Line4.text = lifeform;
        }
    }
}
