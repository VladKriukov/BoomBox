using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    [SerializeField] enum TweenType { Scale, Move, Fade, Rotate };
    [SerializeField] TweenType tweenType;
    [SerializeField] GameObject tweenTarget;
    //[SerializeField] bool enableTarget;
    [SerializeField] bool playParticles;

    [SerializeField] LeanTweenType tweenOn;
    [SerializeField] float tweenOnTime;
    [SerializeField] float tweenOnDelay;
    [SerializeField] Vector3 tweenOnTo;
    [Range(0, 1)] [SerializeField] float fadeOnTo;

    [SerializeField] LeanTweenType tweenOff;
    [SerializeField] float tweenOffTime;
    [SerializeField] float tweenOffDelay;
    [SerializeField] Vector3 tweenOffTo;
    [Range(0, 1)] [SerializeField] float fadeOffTo;

    enum TweenFinishedEvents { TweenedOn, TweenedOff };

    LeanTweenType currentTweenType;
    Vector3 currentTweenTo;
    float currentTweenTime;
    float currentTweenDelay;
    float currentFadeTo;

    void Start()
    {
        if (tweenTarget == null)
        {
            tweenTarget = gameObject;
        }
        
    }

    public void TweenOn()
    {
        currentTweenType = tweenOn;
        currentTweenTo = tweenOnTo;
        currentTweenTime = tweenOnTime;
        currentTweenDelay = tweenOnDelay;
        currentFadeTo = fadeOnTo;
        //Debug.Log("Set tweening on for " + tweenTarget.name);
        //LeanTween.addListener((int)TweenFinishedEvents.TweenedOn, TweenFinished);
        
        DoTween();
    }

    public void TweenOff()
    {
        currentTweenType = tweenOff;
        currentTweenTo = tweenOffTo;
        currentTweenTime = tweenOffTime;
        currentTweenDelay = tweenOffDelay;
        currentFadeTo = fadeOffTo;
        //Debug.Log("Set tweening off for " + tweenTarget.name);
        //LeanTween.addListener((int)TweenFinishedEvents.TweenedOff, TweenFinished);
        DoTween();
    }

    void DoTween()
    {
        /*
        if (enableTarget == true)
        {
            tweenTarget.SetActive(true);
        }
        */
        //Debug.Log("Tweening " + tweenTarget.name);
        switch (tweenType)
        {
            case TweenType.Scale:
                LeanTween.scale(tweenTarget, currentTweenTo, currentTweenTime).setEase(currentTweenType).setDelay(currentTweenDelay).setOnComplete(TweenFinished);
                break;
            case TweenType.Move:
                LeanTween.moveLocal(tweenTarget, currentTweenTo, currentTweenTime).setEase(currentTweenType).setDelay(currentTweenDelay).setOnComplete(TweenFinished);
                break;
            case TweenType.Fade:
                LeanTween.alpha(tweenTarget, currentFadeTo, currentTweenTime).setEase(currentTweenType).setDelay(currentTweenDelay).setOnComplete(TweenFinished);
                break;
            case TweenType.Rotate:
                LeanTween.rotate(tweenTarget, currentTweenTo, currentTweenTime).setEase(currentTweenType).setDelay(currentTweenDelay).setOnComplete(TweenFinished);
                break;
            default:
                break;
        }
    }

    void TweenFinished()
    {
        //Debug.Log("Tween finished event received!");
        if (playParticles)
        {
            tweenTarget.GetComponent<ParticleSystem>().Play();
        }
    }

}
