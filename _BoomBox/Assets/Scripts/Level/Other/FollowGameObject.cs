using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{

    [SerializeField] Vector3 offset;
    [SerializeField] Transform gameObjectToFollow;

    // Update is called once per frame
    void Update()
    {
        if (gameObjectToFollow!=null)
            transform.position = gameObjectToFollow.position + offset;
    }
}
