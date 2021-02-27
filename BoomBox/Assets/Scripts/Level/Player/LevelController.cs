using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using System.Collections.Generic;
using TMPro;

public class LevelController : MonoBehaviour
{

    ///TODO / ideas: different platform types (++*movable*, ++*conveyor belt maybe*?, portal?, key and lock door (or button door), 
    ///++anti bonus block (removes all bonuses)?)
    ///different collectables for score
    ///++timer for speedrunning
    ///++Main menu: stats
    ///++normal particles
    ///sfx
    ///more visuals (UI in game elements like powerup displays (possibly))
    ///++dangers
    ///++restart level if hit danger or fall out of the map
    ///++show level ending before the player starts and then start the game
    ///++save progress, score and time taken to beat level
    ///--bomb placement is in a grid
    ///powerups (++*no time delay for bomb for several clicks*?, ++levitation?, ++magnet?, 
    /// respawn all bonuses bonus?, shield?, ++remove all dangers), ++*possibly respawn after some time*
    ///++bonuses visuals
    ///++remove all bonuses when player dies
    ///++death counter (least or total), move counter (least or total)
    ///++menu options to show stats
    ///++check points
    ///TUTORIAL

    [SerializeField] GameObject levelSelection;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject continueButton;
    int currentNumberOfUnlockedLevels;
    public int currentScene;

    [SerializeField] Color[] colours;
    [SerializeField] Material particleMaterial;
    [SerializeField] Transform cam;

    public delegate void OnLevelLoaded();
    public static OnLevelLoaded levelLoaded;

    void OnEnable()
    {
        BoxBehaviour.levelFinish += LevelComplete;
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameSaver.onDeleteSave += LockAllLevels;
    }

    void OnDisable()
    {
        BoxBehaviour.levelFinish -= LevelComplete;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameSaver.onDeleteSave -= LockAllLevels;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("CurrentPlayerLevel"))
        {
            currentNumberOfUnlockedLevels = PlayerPrefs.GetInt("CurrentPlayerLevel");
            playButton.SetActive(false);
            continueButton.SetActive(true);
        }

        CheckFinalLevel();

        UnlockLevelButtons();
    }

    void UnlockLevelButtons()
    {
        for (int i = 0; i < currentNumberOfUnlockedLevels + 1; i++)
        {
            UnlockLevel(i);
        }
    }

    void UnlockLevel(int i)
    {
        if (i < levelSelection.transform.childCount)
        {
            //Debug.Log("i = " + i + ", levelSelection.transform.childCount = " + levelSelection.transform.childCount);
            levelSelection.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }

    void LockAllLevels()
    {
        for(int i = 1; i < levelSelection.transform.childCount; i++)
        {
            levelSelection.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentScene);
    }

    public void LoadLevel(int num)
    {
        SceneManager.LoadScene(num);
        currentScene = num;
    }

    public void Continue()
    {
        if (currentNumberOfUnlockedLevels < levelSelection.transform.childCount)
            LoadLevel(currentNumberOfUnlockedLevels + 1);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(currentScene + 1);
        currentScene ++;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "_MainMenu")
        {
            levelLoaded?.Invoke();
            cam.GetComponent<Camera>().backgroundColor = colours[currentScene];
            var colorOverLifetime = cam.GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime;
            colorOverLifetime.color = colours[currentScene];
            particleMaterial.color = colours[currentScene];
        }
        Analytics.CustomEvent("level_start", new Dictionary<string, object>
        {
            { "level_index", currentScene }
        });
    }

    void LevelComplete()
    {

        //show player stats: stars defined by how much collectables have been collected
        //switch to next level (after showing player stats for this level)
        //unlock next level
        if (currentScene > currentNumberOfUnlockedLevels)
        {
            currentNumberOfUnlockedLevels = currentScene;
            UnlockLevel(currentNumberOfUnlockedLevels);
            PlayerPrefs.SetInt("CurrentPlayerLevel", currentNumberOfUnlockedLevels);
            PlayerPrefs.Save();
            CheckFinalLevel();
        }
    }

    void CheckFinalLevel()
    {
        if (currentNumberOfUnlockedLevels >= levelSelection.transform.childCount)
        {
            playButton.SetActive(true);
            continueButton.SetActive(false);
            playButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Play again";
        }
    }
}