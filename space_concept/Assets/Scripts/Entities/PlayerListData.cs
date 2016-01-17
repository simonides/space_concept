using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerListData {

    public PlayerData HumanPlayer { get; private set; }
    public List<AiPlayer> AiPlayers { get; private set; }

    //public PlayerListData() {
    //    HumanPlayer = new PlayerData();
    //    AiPlayers = new List<AiPlayer>();
    //}

    private Color ColorDez(int r, int g, int b) {
        return new Color(r / 255f, g / 255f, b / 255f);
    }

    void InitPredefinedPlayerValues() {
        predefinedPlayerColors = new List<Color> {  Color.blue, Color.red,
            ColorDez( 178, 0, 255), ColorDez( 255, 106, 0),ColorDez( 255, 216, 0),
            ColorDez( 182, 255, 0),ColorDez( 0, 255, 144),ColorDez( 0, 255, 255),ColorDez( 0, 148, 355),ColorDez( 72, 0, 255),
            ColorDez( 255, 0, 220),ColorDez( 127, 51, 0),ColorDez( 127, 106, 0),ColorDez( 0, 127, 127),ColorDez(127, 0, 110),
        };
        predefinedPlayerNames = new List<string> { "Adriatik", "Alemmania", "Paparossa", "Aldarin", "Dimitri", "Bluna", "Champagnus", "Emilia-Extra", "Godpower", "Hopper",
            "Jura", "Kleria", "Lepas", "Legara", "Clersa", "Magnora", "Cercela", "Phoenix", "Flocki", "Precious", "Prim",
            "Shamante", "Siebiz", "Berte", "Smudo", "Smnomen", "Sultra", "Tarbata", "Topas", "Viktualia", "Wasa" };
    }
    //seralization needs an empty constructor
    public PlayerListData() {
        InitPredefinedPlayerValues();
    }

    public PlayerListData(PlayerData humanPlayer, List<AiPlayer> aiPlayers) {
        InitPredefinedPlayerValues();
        this.HumanPlayer = humanPlayer;
        this.AiPlayers = aiPlayers;
        if (HumanPlayer.Color == Color.black) {
            HumanPlayer.Color = GetUniqueRandomPlayerColor();
        } else {
            ColorAlreadyInUse(HumanPlayer.Color);
        }
        if (HumanPlayer.Name == "") {
            HumanPlayer.Name = GetUniqueRandomPlayerName();
        } else {
            NameAlreadyInUse(HumanPlayer.Name);
        }
        foreach (AiPlayer ai in aiPlayers) {
            if (ai.playerData.Color == Color.black) {
                ai.playerData.Color = GetUniqueRandomPlayerColor();
            } else {
                ColorAlreadyInUse(ai.playerData.Color);
            }
            if (ai.playerData.Name == "") {
                ai.playerData.Name = GetUniqueRandomPlayerName();
            } else {
                NameAlreadyInUse(ai.playerData.Name);
            }
        }
    }

    public PlayerData GetPlayerByName(string playerName) {
        if (HumanPlayer.Name.Equals(playerName)) {
            return HumanPlayer;
        }
        foreach (AiPlayer ai in AiPlayers) {
            if (ai.playerData.Name.Equals(playerName)) {
                return ai.playerData;
            }
        }
        return null;
    }


    static List<Color> predefinedPlayerColors;
    static List<string> predefinedPlayerNames;

    static public Color GetUniqueRandomPlayerColor() {
        Debug.Assert(predefinedPlayerColors.Count > 0);
        int idx = Random.Range(0, predefinedPlayerColors.Count);
        Color color = predefinedPlayerColors[idx];
        predefinedPlayerColors.RemoveAt(idx);
        return color;
    }

    static public void ColorAlreadyInUse(Color color) {
        predefinedPlayerColors.Remove(color);
    }

    static public string GetUniqueRandomPlayerName() {
        Debug.Assert(predefinedPlayerNames.Count > 0);
        int idx = Random.Range(0, predefinedPlayerNames.Count);
        string name = predefinedPlayerNames[idx];
        predefinedPlayerNames.RemoveAt(idx);
        return name;
    }
    static public void NameAlreadyInUse(string name) {
        predefinedPlayerNames.Remove(name);
    }



}

