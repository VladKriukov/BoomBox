using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSaverOld : MonoBehaviour
{

    [SerializeField] Transform levelSelection;
    [SerializeField] StatsSorter statsSorter;
    [SerializeField] LevelController levelController;
    [SerializeField] CanvasController canvasController;
    [SerializeField] bool deleteEverything;

    int currentLevelIndex;
    List<int> levelStars = new List<int>();
    List<int> levelHighscore = new List<int>();
    List<float> levelBestTime = new List<float>();
    List<int> levelDeathsTotal = new List<int>();
    List<int> levelLeastDeaths = new List<int>();
    List<int> levelMovesTotal = new List<int>();
    List<int> levelLeastMoves = new List<int>();

    Transform child;

    void Start()
    {
        if (deleteEverything)
        {
            PlayerPrefs.DeleteAll();
        }
        for (int i = 0; i < levelSelection.childCount; i++)
        {
            child = levelSelection.GetChild(i);
            child.Find("LevelNumber").GetComponent<TextMeshProUGUI>().SetText("" + (i + 1));
            levelStars.Add(0);
            levelHighscore.Add(0);
            levelBestTime.Add(1999999999);
            levelDeathsTotal.Add(0);
            levelLeastDeaths.Add(1999999999);
            levelMovesTotal.Add(0);
            levelLeastMoves.Add(1999999999);
        }
        //LoadAll();
    }

    public void UpdateStats(int thisLevelStars, int thisLevelScore, float thisLevelTime, int thisLevelDeaths, int thisLevelMoves)
    {
        currentLevelIndex = levelController.currentScene - 1;

        Debug.Log("Updating stats for level " + currentLevelIndex + ". Stars: " + thisLevelStars + "" +
            ", Score: " + thisLevelScore + ", Time: " + thisLevelTime);

        //Debug.Log("Current level previous stats: Stars: " + levelStars[currentLevelIndex] + "" +
        //    ", Score: " + levelHighscore[currentLevelIndex] + ", Time: " + levelBestTime[currentLevelIndex]);
        if (levelSelection.childCount > currentLevelIndex)
        {
            child = levelSelection.GetChild(currentLevelIndex);
            if (thisLevelStars > levelStars[currentLevelIndex])
            {
                levelStars[currentLevelIndex] = thisLevelStars;
            }
            if (thisLevelScore > levelHighscore[currentLevelIndex])
            {
                levelHighscore[currentLevelIndex] = thisLevelScore;
                Debug.Log("New highscore on level " + currentLevelIndex + " of " + thisLevelScore + "!");
            }
            if (thisLevelTime < levelBestTime[currentLevelIndex])
            {
                Debug.Log("New best time on level " + currentLevelIndex + " of " + thisLevelTime + "! Last time was: " + levelBestTime[currentLevelIndex]);
                levelBestTime[currentLevelIndex] = thisLevelTime;
            }
            if (thisLevelDeaths < levelLeastDeaths[currentLevelIndex]){
                levelLeastDeaths[currentLevelIndex] = thisLevelDeaths;
                Debug.Log("New best on deaths on level " + currentLevelIndex + " of " + thisLevelDeaths + "!");
            }
            if(thisLevelMoves < levelLeastMoves[currentLevelIndex]){
                levelLeastMoves[currentLevelIndex] = thisLevelMoves;
                Debug.Log("New best moves on level " + currentLevelIndex + " of " + thisLevelMoves + "!");
            }

            levelDeathsTotal[currentLevelIndex] += thisLevelDeaths;
            levelMovesTotal[currentLevelIndex] += thisLevelMoves;

            ActivateStars(thisLevelStars);
            ShowStats(currentLevelIndex);
        }
        else
        {
            Debug.Log("This is the last level. Trying to update stuff on child: " + currentLevelIndex + ", total number of children on level selection (1 more than GetChild()): " + levelSelection.childCount);
        }
        
        
        Save();
    }

    void ShowStats(int index)
    {
        Debug.Log("Showing stats for level: " + index + ", level time is: " + levelBestTime[index]);
        child.Find("LevelHighscore").GetComponent<TextMeshProUGUI>().SetText("Highscore: " + levelHighscore[index]);
        child.Find("LevelBestTime").GetComponent<TextMeshProUGUI>().SetText("Best time: " + canvasController.FormatTime(levelBestTime[index]));
        // todo level moves / deaths, least & total
        statsSorter.FillStats(levelHighscore, levelBestTime, levelLeastDeaths, levelDeathsTotal, levelLeastMoves, levelMovesTotal);
    }

    void ActivateStars(int num)
    {
        for (int i = 0; i < num; i++)
        {
            child.Find("LevelStars/StarBackgroundImage (" + (i + 1) + ")/Star").gameObject.SetActive(true);
        }
    }

    void Save()
    {
        Debug.Log("Saving game for level: " + currentLevelIndex);
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "Stars", levelStars[currentLevelIndex]);
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "Score", levelHighscore[currentLevelIndex]);
        PlayerPrefs.SetFloat("Level" + currentLevelIndex + "Time", levelBestTime[currentLevelIndex]);
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "LeastDeaths", levelLeastDeaths[currentLevelIndex]);
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalDeaths", levelDeathsTotal[currentLevelIndex]);
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "LeastMoves", levelLeastMoves[currentLevelIndex]);
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalMoves", levelMovesTotal[currentLevelIndex]);
        PlayerPrefs.Save();
    }

    void LoadAll()
    {
        if (PlayerPrefs.HasKey("Level0Stars"))
        {
            Debug.Log("Loading game...");
            for (int i = 0; i < levelSelection.childCount; i++)
            {
                Debug.Log("Loading stats for level " + i);
                levelStars[i] = PlayerPrefs.GetInt("Level" + i + "Stars");
                levelHighscore[i] = PlayerPrefs.GetInt("Level" + i + "Score");
                levelBestTime[i] = PlayerPrefs.GetFloat("Level" + i + "Time");
                levelLeastDeaths[i] = PlayerPrefs.GetInt("Level" + i + "LeastDeaths");
                levelDeathsTotal[i] = PlayerPrefs.GetInt("Level" + i + "TotalDeaths");
                levelLeastMoves[i] = PlayerPrefs.GetInt("Level" + i + "LeastMoves");
                levelMovesTotal[i] = PlayerPrefs.GetInt("Level" + i + "TotalMoves");
                if (levelBestTime[i] == 0)
                {
                    // this level was not played
                    levelBestTime[i] = Mathf.Infinity;
                    levelLeastDeaths[i] = Mathf.RoundToInt(Mathf.Infinity);
                    levelLeastMoves[i] = Mathf.RoundToInt(Mathf.Infinity);
                }
            }
            Invoke("ShowLoadedThings", 0.25f);
        }
    }

    void ShowLoadedThings()
    {
        for (int i = 0; i < levelSelection.childCount; i++)
        {
            child = levelSelection.GetChild(i);
            Debug.Log("Level: " +i+", Stars: " + levelStars[i] + ", Highscore: " + levelHighscore[i]+ ", Best time: " + levelBestTime[i]);
            if (levelBestTime[i] != Mathf.Infinity)
            {
                ShowStats(i);
                ActivateStars(levelStars[i]);
            }
        }
        
    }

}
