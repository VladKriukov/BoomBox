using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float power = 10;
    [SerializeField] float radius = 3;
    [SerializeField] float upwardsModifier = 2;
    TimerNode explosionTimer;

    public delegate void OnMove();
    public static OnMove onMove;

    void Start()
    {
        explosionTimer = GetComponent<TimerNode>();
        if (GameSettings.bombUpwardsModifier == false)
        {
            upwardsModifier = 0;
        }
    }

    public void BombPlaced()
    {
        Vector3 pos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(pos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(power, pos, radius, upwardsModifier);
            }
            if (hit.gameObject.name == "Player")
            {
                onMove?.Invoke();
            }
            explosionTimer.StartTimer();
            if (GameSettings.sfxOn) transform.GetChild(0).gameObject.SetActive(true);
        }
    }

}
