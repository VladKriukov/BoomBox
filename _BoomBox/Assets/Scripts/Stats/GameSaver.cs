using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaver : MonoBehaviour
{

    //List<string[]> saveArrays = new List<string[]>();

    string[] saveSplit;

    public delegate void OnDeleteSave();
    public static OnDeleteSave onDeleteSave;

    public delegate void SaveComplete();
    public static SaveComplete OnSaveComplete;

    void Start() {
        Invoke("LoadAll", 0.05f);
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        onDeleteSave?.Invoke();
        Debug.Log("Saves deleted");
    }

    public void Save()
    {
        for(int i = 0; i < Stats.save.Count; i++)
        {
            PlayerPrefs.SetString("Level" + i + "Save", Stats.save[i]);
        }
        Debug.Log("Saving all");
        PlayerPrefs.Save();
        OnSaveComplete?.Invoke();
    }

    void LoadAll()
    {
        if(PlayerPrefs.HasKey("Level0Save"))
        {
            Stats.save.Clear();
            for(int i = 0; i < Stats.numberOfLevels; i++)
            {
                Stats.save.Add(PlayerPrefs.GetString("Level" + i + "Save"));
                saveSplit = Stats.save[i].Split("|"[0]);
                Stats.levelStars[i] = int.Parse(saveSplit[1]);
                Stats.highscores[i] = int.Parse(saveSplit[2]);
                Stats.bestTimes[i] = float.Parse(saveSplit[3]);
                Stats.lowestDeaths[i] = int.Parse(saveSplit[4]);
                Stats.lowestMoves[i] = int.Parse(saveSplit[5]);
                Stats.totalDeaths[i] = int.Parse(saveSplit[6]);
                Stats.totalMoves[i] = int.Parse(saveSplit[7]);
                //Debug.Log("Loading stats for level " + saveSplit[0] + ": " + Stats.save[i]);
            }
            
        }
    }

}
