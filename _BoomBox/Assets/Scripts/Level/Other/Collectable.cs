using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    [SerializeField] int bonusScore;
    public static int collectableAmount { get; private set; }

    public delegate void OnCollected(int amount);
    public static OnCollected onCollected;

    private void OnEnable() {
        collectableAmount++;
    }

    private void OnDisable() {
        collectableAmount--;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && BoxBehaviour.inGame)
        {
            onCollected?.Invoke(bonusScore);
            //Debug.Log("Player Collected Bonus. Adding score of " + bonusScore);
            //gameObject.SetActive(false);
            if (GameSettings.sfxOn) transform.GetChild(0).gameObject.SetActive(true);
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            enabled = false;
        }
    }

}
