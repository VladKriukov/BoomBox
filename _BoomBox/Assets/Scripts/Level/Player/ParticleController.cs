using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    int indexer;
    List<Transform> hitParticles = new List<Transform>();

    void OnEnable() {
        BoxBehaviour.hit += EnableHitPatricle;
    }

    void OnDisable() {   
        BoxBehaviour.hit -= EnableHitPatricle;
    }

    void Start() {
        foreach (Transform item in transform)
        {
            hitParticles.Add(item);
            if (!GameSettings.sfxOn) item.GetComponent<AudioSource>().enabled = false;
        }
    }

    void EnableHitPatricle(Vector3 pos)
    {
        //if (GameSettings.shakeOn == false) return;
        
        hitParticles[indexer].gameObject.SetActive(true);
        hitParticles[indexer].position = pos;
        indexer++;
        if (indexer == hitParticles.Count)
        {
            indexer = 0;
        }
    }

}
