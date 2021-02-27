using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;

public class Stats : MonoBehaviour
{

    [SerializeField] LevelController levelController;
    [SerializeField] Transform levelSelectionReference;
    [SerializeField] TextMeshProUGUI clearedBonus;
    [SerializeField] TextMeshProUGUI noDeathBonus;
    [SerializeField] TextMeshProUGUI extraBonus;
    public static int currentLevelStars { get; private set; }
    public static int levelScore { get; private set; }
    public static float levelTime { get; private set; }
    public static string formattedTime { get; private set; }
    public static int levelDeaths { get; private set; }
    public static int levelMoves { get; private set; }
    int currentLevel;

    public static List<int> levelStars = new List<int>();
    public static List<int> highscores = new List<int>();
    public static List<float> bestTimes = new List<float>();
    public static List<int> lowestDeaths = new List<int>();
    public static List<int> totalDeaths = new List<int>();
    public static List<int> lowestMoves = new List<int>();
    public static List<int> totalMoves = new List<int>();

    public static List<string> save { get; private set; } = new List<string>();
    public static int numberOfLevels { get; private set; }
    bool didNotDie;
    bool collectedAllCollectables;

    public UnityEngine.Events.UnityEvent OnLevelCleared;
    public UnityEngine.Events.UnityEvent OnLevelNoDeaths;
    public UnityEngine.Events.UnityEvent OnUpdateExtraScore;

    void OnEnable() {
        CameraController.levelStart += LevelStarted;
        //BoxBehaviour.levelFinish += LevelFinished;
        Collectable.onCollected += AddScore;
        BoxBehaviour.onDeath += AddDeath;
        Bomb.onMove += AddMove;
        StarCalculator.playerStars += SetLevelStars;
        GameSaver.onDeleteSave += ClearStats;
    }

    void OnDisable() {
        CameraController.levelStart -= LevelStarted;
        //BoxBehaviour.levelFinish -= LevelFinished;
        Collectable.onCollected -= AddScore;
        BoxBehaviour.onDeath -= AddDeath;
        Bomb.onMove -= AddMove;
        StarCalculator.playerStars -= SetLevelStars;
        GameSaver.onDeleteSave -= ClearStats;
    }

    void Start() {
        numberOfLevels = levelSelectionReference.childCount;
        ClearStats();
    }

    void ClearStats()
    {
        levelStars.Clear();
        highscores.Clear();
        bestTimes.Clear();
        totalDeaths.Clear();
        lowestDeaths.Clear();
        totalMoves.Clear();
        lowestMoves.Clear();
        for(int i = 0; i < numberOfLevels; i++)
        {
            SetUpStats();
        }
    }

    void SetUpStats()
    {
        levelStars.Add(0);
        highscores.Add(0);
        bestTimes.Add(1999999999);
        totalDeaths.Add(0);
        lowestDeaths.Add(1999999999);
        totalMoves.Add(0);
        lowestMoves.Add(1999999999);
    }

    void Update()
    {
        if (BoxBehaviour.inGame == true)
        {
            levelTime += Time.deltaTime;

            formattedTime = FormatTime(levelTime);
        }
    }

    void LevelStarted(){
        ResetStats();
    }

    void AddScore(int amount){
        levelScore += amount;
    }

    void AddDeath(){
        levelDeaths++;
    }

    void AddMove(){
        levelMoves++;
    }

    void SetLevelStars(int amount){
        currentLevelStars = amount;
    }

    void LevelFinished(){
        CompareStats();
        FormatSave();
    }

    public IEnumerator CheckForExtraScore()
    {
        int bonusScore = 0;
        if(Collectable.collectableAmount == 0)
        {
            collectedAllCollectables = true;
            bonusScore = Mathf.RoundToInt((levelScore / 3) + 5);
            levelScore += bonusScore;
            clearedBonus.SetText("+" + bonusScore);
            clearedBonus.GetComponent<Tweener>().TweenOn();
            clearedBonus.GetComponent<Tweener>().TweenOff();
            OnUpdateExtraScore.Invoke();
            OnLevelCleared.Invoke();
        }
        yield return new WaitForSeconds(0.45f);
        
        if(levelDeaths == 0)
        {
            didNotDie = true;
            bonusScore = Mathf.RoundToInt((levelScore / 2) + 4);
            levelScore += bonusScore;
            noDeathBonus.SetText("+" + bonusScore);
            noDeathBonus.GetComponent<Tweener>().TweenOn();
            noDeathBonus.GetComponent<Tweener>().TweenOff();
            OnUpdateExtraScore.Invoke();
            OnLevelNoDeaths.Invoke();
        }
        yield return new WaitForSeconds(0.75f);

        if (didNotDie && collectedAllCollectables)
        {
            Debug.Log("You madlad! You cleared the level AND did not die a single time!");
            bonusScore = Mathf.RoundToInt(levelScore / 2);
            levelScore += bonusScore;
            extraBonus.SetText("+" + bonusScore);
            extraBonus.GetComponent<Tweener>().TweenOn();
            extraBonus.GetComponent<Tweener>().TweenOff();
            OnUpdateExtraScore.Invoke();
            /*
            Analytics.CustomEvent("level_complete", new Dictionary<string, object>
            {
                { "level_index", currentLevel },
            });
            */
        }
        LevelFinished();
        yield return null;
    }

    void CompareStats(){
        currentLevel = levelController.currentScene - 1;
        if(currentLevelStars > levelStars[currentLevel])
        {
            levelStars[currentLevel] = currentLevelStars;
        }
        if(levelScore > highscores[currentLevel])
        {
            highscores[currentLevel] = levelScore;
            Debug.Log("New highscore!");
        }
        if(levelTime < bestTimes[currentLevel])
        {
            bestTimes[currentLevel] = levelTime;
            Debug.Log("New best time!");
        }
        if(levelDeaths < lowestDeaths[currentLevel])
        {
            lowestDeaths[currentLevel] = levelDeaths;
            Debug.Log("New lowest death!");
        }
        if(levelMoves < lowestMoves[currentLevel])
        {
            lowestMoves[currentLevel] = levelMoves;
            Debug.Log("New least moves!");
        }

        totalDeaths[currentLevel] += levelDeaths;
        totalMoves[currentLevel] += levelMoves;
    }

    void FormatSave(){
        save.Clear();
        Debug.Log("Formatting save.");
        for(int i = 0; i < levelStars.Count; i++)
        {
            save.Add("" + (i + 1) + "|" + levelStars[i] + "|" + highscores[i] + "|" + bestTimes[i] + "|" + lowestDeaths[i] + "|" +
            "" + lowestMoves[i] + "|" + totalDeaths[i] + "|" + totalMoves[i]);
            //Debug.Log("Save: " + save[i]);
        }
        GetComponent<GameSaver>().Save();
        Analytics.CustomEvent("level_complete", new Dictionary<string, object>
        {
            { "level_index", currentLevel },
            { "level_stars", currentLevelStars },
            { "level_score", levelScore},
            { "level_time", levelTime },
            { "level_deaths", levelDeaths },
            { "level_moves", levelMoves }
        });
    }

    void ResetStats(){
        currentLevelStars = 0;
        levelScore = 0;
        levelTime = 0f;
        levelDeaths = 0;
        levelMoves = 0;
        didNotDie = false;
        collectedAllCollectables = false;
    }

    static string minutes;
    static string seconds;

    public static string FormatTime(float timeToFormat)
    {
        minutes = Mathf.Floor(timeToFormat / 60).ToString("00");
        seconds = (timeToFormat % 60).ToString("00");
        //milliseconds = "" + Mathf.Floor((timeToFormat * 100) % 100);
        string milliseconds2 = "" + (timeToFormat - Mathf.Floor(timeToFormat));
        milliseconds2 = milliseconds2.Substring(2, 2);
        string newFormattedTime = string.Format("{0}:{1}.{2}", minutes, seconds, milliseconds2);
        return newFormattedTime;
    }

}