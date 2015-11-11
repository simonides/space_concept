﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 *  Player Entity.
 *  Contains all information about a single player.
 *  Can be serialised / deserialised to a file.
 */
public class PlayerData {
    public string Name { get; set; } 
    public Color Color { get; set; }

    static List<string> predefinedPlayerNames = new List<string> { "Adriatik", "Alemmania", "Apple", "Papa Schlumpf", "Aladin", "Dimitridis", "Bluna", "Champagna", "Emilia-Extra", "Godpower", "Hope",
                                                                    "Junior", "Klee", "Laser", "Legolas", "London", "Magic", "Pepsi-Carola", "Phoenix", "Popo", "Precious", "Pumuckl", "Schneewittchen",
                                                                    "Schokominza", "Siebenstern", "Sioux", "Smudo", "Sonne", "Sultan", "Tarzan", "Topas", "Viktualia", "Wasa" };

    string GetRandomPlayerName() {
        return predefinedPlayerNames[UnityEngine.Random.Range(0, predefinedPlayerNames.Count)];
    }
    
    public override string ToString() {
        return "Player \"" + Name + "\", color " + Color.ToString();
    }
}
