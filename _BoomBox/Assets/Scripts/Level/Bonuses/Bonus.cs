using System;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public enum BonusType
    {
        bombCountBonus, levitation, removeAllDangers, magnet, antiBonus
    }
    public BonusType bonusType;
    public Effect effect;
    TimerNode timerNode;

    void Start() {
        timerNode = GetComponent<TimerNode>();
        ActivateBonus(true);
    }

    public void ActivateBonus(bool value){
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }
        switch (bonusType)
        {
            case BonusType.bombCountBonus:
                transform.GetChild(0).gameObject.SetActive(value);
                break;
            case BonusType.levitation:
                transform.GetChild(1).gameObject.SetActive(value);
                break;
            case BonusType.removeAllDangers:
                transform.GetChild(2).gameObject.SetActive(value);
                break;
            case BonusType.magnet:
                transform.GetChild(3).gameObject.SetActive(value);
                break;
            case BonusType.antiBonus:
                transform.GetChild(4).gameObject.SetActive(value);
                break;
            default:
                break;
        }
    }

    public void StartTimer()
    {
        timerNode.StartTimer();
    }
}

[Serializable]
public class Effect
{
    public int effectAmount;
    public float effectTime;

}