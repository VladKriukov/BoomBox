using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI levelScoreStat;
    [SerializeField] TextMeshProUGUI levelTimeStat;
    [SerializeField] LevelController levelController;
    [SerializeField] Stats stats;
    //[SerializeField] TextMeshProUGUI levelComplete;
    [SerializeField] Tweener star1;
    [SerializeField] Tweener star2;
    [SerializeField] Tweener star3;
    [SerializeField] GameObject buttonRestartLevel;
    [SerializeField] GameObject buttonSelectLevel;
    [SerializeField] GameObject buttonNextLevel;
    Transform levelSelection;
    int levelScore;

    string minutes;
    string seconds;
    //string milliseconds;

    public UnityEngine.Events.UnityEvent OnLevelLoaded;
    public UnityEngine.Events.UnityEvent OnLevelStarted;
    public UnityEngine.Events.UnityEvent OnLevelFinished;

    void OnEnable()
    {
        levelSelection = transform.Find("LevelSelection/Scroll View/Viewport/Content");
        LevelController.levelLoaded += ResetLevelStats;
        CameraController.levelStart += StartLevel;
        BoxBehaviour.levelFinish += FinishLevel;
        Collectable.onCollected += CollectedBonus;
        StarCalculator.playerStars += ShowStars;
    }
    
    void OnDisable()
    {
        LevelController.levelLoaded -= ResetLevelStats;
        CameraController.levelStart -= StartLevel;
        BoxBehaviour.levelFinish -= FinishLevel;
        Collectable.onCollected -= CollectedBonus;
        StarCalculator.playerStars -= ShowStars;
    }

    void ShowStars(int num)
    {
        switch (num)
        {
            case 1:
                star1.TweenOn();
                Invoke("ShowExtras", 1.3f);
                break;
            case 2:
                star1.TweenOn();
                star2.TweenOn();
                Invoke("ShowExtras", 1.9f);
                break;
            case 3:
                star1.TweenOn();
                star2.TweenOn();
                star3.TweenOn();
                Invoke("ShowExtras", 2.5f);
                break;
            default:
                Invoke("ShowExtras", 2.5f);
                break;
        }
        ShowLevelStats();
    }

    void ShowExtras()
    {
        stats.StartCoroutine("CheckForExtraScore");
        // send for check extra score on stats
        Invoke("EnableButtons", 1.4f);

    }

    void EnableButtons()
    {
        buttonRestartLevel.SetActive (true);
        buttonSelectLevel.SetActive (true);
        if (levelController.currentScene < levelSelection.childCount)
        {
            buttonNextLevel.SetActive(true);
        }
    }

    void ResetLevelStats()
    {
        OnLevelLoaded.Invoke();
        levelScore = 0;
        CollectedBonus(0);
        buttonRestartLevel.SetActive(false);
        buttonSelectLevel.SetActive(false);
        buttonNextLevel.SetActive(false);
    }

    void StartLevel()
    {
        OnLevelStarted.Invoke();
    }

    void FinishLevel()
    {
        OnLevelFinished.Invoke();
    }

    void Update()
    {
        if (BoxBehaviour.inGame == true)
        {
            timerText.SetText("Time: " + Stats.formattedTime);
        }
    }

    void CollectedBonus(int amount)
    {
        levelScore += amount;
        scoreText.SetText("Score: " + levelScore);
    }

    public void UpdateBonusPoints()
    {
        levelScoreStat.SetText("Level score: " + Stats.levelScore);
    }

    void ShowLevelStats()
    {
        //Debug.Log("Showing level stats. Score: " + Stats.levelScore + ", Time: " + Stats.levelTime + ", Formatted time: " + Stats.formattedTime);
        levelScoreStat.SetText("Level score: " + Stats.levelScore);
        levelTimeStat.SetText("Level time: " + Stats.formattedTime);
    }

    public string FormatTime(float timeToFormat)
    {
        minutes = Mathf.Floor(timeToFormat / 60).ToString("00");
        seconds = (timeToFormat % 60).ToString("00");
        //milliseconds = "" + Mathf.Floor((timeToFormat * 100) % 100);
        string milliseconds2 = "" + (timeToFormat - Mathf.Floor(timeToFormat));
        milliseconds2 = milliseconds2.Substring(2, 2);
        string newFormattedTime = string.Format("{0}:{1}.{2}", minutes, seconds, milliseconds2);
        return newFormattedTime;

        //Debug.Log("Milliseconds 2: " + milliseconds2 + ", time to format given: " + timeToFormat);
        //Debug.Log("Time in float: " + timeToFormat + ", minutes: " + minutes + ", seconds: " + seconds + ", milliseconds: " + milliseconds + ", milliseconds 2: " + milliseconds2);

    }

}
