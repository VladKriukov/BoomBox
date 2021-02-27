using UnityEngine;
using TMPro;

public class StatFiller : MonoBehaviour
{
	bool isFullStats;

	void OnEnable()
	{
		GameSaver.onDeleteSave += MiniWait;
		GameSaver.OnSaveComplete += FillStats;
	}

	void OnDisable()
	{
		GameSaver.onDeleteSave -= MiniWait;
		GameSaver.OnSaveComplete -= FillStats;
	}

	void Start()
	{
		if (transform.GetChild(0).childCount > 4)
		{
			isFullStats = true;
		}
		MiniWait();
	}

	void MiniWait()
	{
		Invoke("FillStats", 0.1f);
	}

	void FillStats()
	{
		Transform currentStat = transform;
		for (int i = 0; i < Stats.numberOfLevels; i++)
		{
			currentStat = transform.GetChild(i);
			currentStat.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("" + (i + 1));
			if (Stats.bestTimes[i] != 2e+09)
			{
				if (isFullStats)
				{
					currentStat.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("" + Stats.highscores[i]);
					currentStat.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("" + Stats.FormatTime(Stats.bestTimes[i]));
					currentStat.GetChild(3).GetComponent<TextMeshProUGUI>().SetText("" + Stats.lowestDeaths[i]);
					currentStat.GetChild(4).GetComponent<TextMeshProUGUI>().SetText("" + Stats.totalDeaths[i]);
					currentStat.GetChild(5).GetComponent<TextMeshProUGUI>().SetText("" + Stats.lowestMoves[i]);
					currentStat.GetChild(6).GetComponent<TextMeshProUGUI>().SetText("" + Stats.totalMoves[i]);
				}
				else
				{
					currentStat.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Highscore: " + Stats.highscores[i]);
					currentStat.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("Best time: " + Stats.FormatTime(Stats.bestTimes[i]));

					Debug.Log("Level " + i + " now have " + Stats.levelStars[i]);
					for (int k = 0; k < Stats.levelStars[i]; k++)
					{
						currentStat.Find("LevelStars/StarBackgroundImage (" + (k + 1) + ")/Star").gameObject.SetActive(true);
					}
				}
			}
			else
			{
				//Debug.Log("Level " + i + " was not played or was deleted");
				if (isFullStats)
				{
					for (int l = 1; l < currentStat.childCount; l++)
					{
						currentStat.GetChild(l).GetComponent<TextMeshProUGUI>().SetText("N/A");
					}
				}
				else
				{
					currentStat.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("");
					currentStat.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("");
					for (int k = 0; k < 3; k++)
					{
						currentStat.Find("LevelStars/StarBackgroundImage (" + (k + 1) + ")/Star").gameObject.SetActive(false);
					}
				}
			}
		}
	}
}
