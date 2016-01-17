using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 *  Player Entity.
 *  Contains all information about a single player.
 *  Can be serialised / deserialised to a file.
 */
[System.Serializable]
public class PlayerData {

    public PlayerData() {
        Name = "";
        Color = Color.black;
        IsHumanPlayer = true;
        ownedPlanets = new List<PlanetData>();
    }
    public string Name { get; set; } 
    public Color Color { get; set; }


    //public List<PlanetData> OwnedPlanets { get; private set; }  // This list is dynamically updated from outside
   [System.NonSerialized]
    private List<PlanetData> ownedPlanets;

    public List<PlanetData> GetOwnedPlanets()
    {
        return ownedPlanets;
    }
    public void SetOwnedPlanets(List<PlanetData> pd)
    {
        ownedPlanets = pd;
    }

    public bool IsHumanPlayer { get; set; }     // True: this is the player who plays the game; false: AI (=enemy)
        
    public void AddPlanetToOwnership(PlanetData planet) {
        if (planet.Owner != this) {
            Debug.LogError("Error: A planet can't be added to the ownership-list of the player, if the player actually doesn't own the planet.");
        }
        ownedPlanets.Add(planet);
    }

    public void RemovePlanetFromOwnership(PlanetData planet) {
        if (planet.Owner == this) {
            Debug.LogError("Error: A planet can't be removed from the ownership-list of the player, if it is still owned by the player.");
        }
        ownedPlanets.Remove(planet);
    }

    public int GetNumberOfOwnedPlanets() {
        return ownedPlanets.Count;
    }
    
    public override string ToString() {
        return "Player \"" + Name + "\", color " + Color.ToString();
    }
}
