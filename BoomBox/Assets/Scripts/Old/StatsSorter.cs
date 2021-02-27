using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsSorter : MonoBehaviour
{

    [SerializeField] GameObject stat;
    CanvasController canvasController;
    
    void Start() {
        canvasController = FindObjectOfType<CanvasController>();    
    }
    
    public void FillStats(List<int> highscore, List<float> bestTime, List<int> deathMin, List<int> deathTotal, List<int> moveMin, List<int> moveTotal)
    {
        for(int i = 0; i < highscore.Count; i++)
        {

            Transform currentChild = null;
            if(transform.childCount < i + 1)
            {
                currentChild = Instantiate(stat, transform).transform;
            }else
            {
                currentChild = transform.GetChild(i);
            }
            currentChild.Find("LevelNumber").GetComponent<TextMeshProUGUI>().SetText("" + (i+1));
            TextMeshProUGUI score = currentChild.Find("LevelScore").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI time = currentChild.Find("LevelTime").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI minDeaths = currentChild.Find("LevelMinDeaths").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI totalDeaths = currentChild.Find("LevelTotalDeaths").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI minMoves = currentChild.Find("LevelMinMoves").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI totalMoves = currentChild.Find("LevelTotalMoves").GetComponent<TextMeshProUGUI>();

            if(bestTime[i] < 1999999999)
            {
                score.SetText("" + highscore[i]);
                Debug.Log("Filling time stat for level " + i + " as " + bestTime[i]);
                time.SetText("" + canvasController.FormatTime(bestTime[i]));
                minDeaths.SetText("" + deathMin[i]);
                totalDeaths.SetText("" + deathTotal[i]);
                minMoves.SetText("" + moveMin[i]);
                totalMoves.SetText("" + moveTotal[i]);
            }else
            {
                score.SetText("N/A");
                time.SetText("N/A");
                minDeaths.SetText("N/A");
                totalDeaths.SetText("N/A");
                minMoves.SetText("N/A");
                totalMoves.SetText("N/A");
            }
            
        }
    }

}
