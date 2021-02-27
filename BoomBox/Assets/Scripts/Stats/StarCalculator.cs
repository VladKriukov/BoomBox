using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCalculator : MonoBehaviour
{
    [Range(0,1)][SerializeField] float oneStarThreshold = 0.9f;
    [Range(0,1)][SerializeField] float twoStarThreshold = 0.581f;
    [Range(0,1)][SerializeField] float threeStarThreshold = 0.353f;
    int initialNumberOfCollectables;
    float remainder = 0f;

    public delegate void OnStarsCalculated(int stars);
    public static OnStarsCalculated playerStars;

    void Start()
    {
        initialNumberOfCollectables = transform.childCount;
    }

    void OnEnable()
    {
        BoxBehaviour.levelFinish += CalculateStars;
    }

    void OnDisable()
    {
        BoxBehaviour.levelFinish -= CalculateStars;
    }

    void CalculateStars()
    {
        if (Collectable.collectableAmount > 0)
        {
            remainder = (float)Collectable.collectableAmount / (float)initialNumberOfCollectables;
            Debug.Log("Initial amount of collectables: " + initialNumberOfCollectables + ", " +
                "remaining collectables: " + Collectable.collectableAmount + ", " + Collectable.collectableAmount + " / " +
                "" + initialNumberOfCollectables + " = " + remainder);
            if (remainder >= oneStarThreshold)
            {
                playerStars?.Invoke(0);
            }
            if (remainder < oneStarThreshold && remainder >= twoStarThreshold)
            {
                playerStars?.Invoke(1);
            }
            if (remainder < twoStarThreshold && remainder >= threeStarThreshold)
            {
                playerStars?.Invoke(2);
            }
            if (remainder < threeStarThreshold)
            {
                playerStars?.Invoke(3);
            }
        }
        else
        {
            playerStars?.Invoke(3);
        }
        
    }

}
