using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSetUp : MonoBehaviour
{

    void OnEnable()
    {
        LevelController.levelLoaded += FindTargetTransforms;
    }

    void OnDisable()
    {
        LevelController.levelLoaded -= FindTargetTransforms;
    }

    void FindTargetTransforms()
    {
        CinemachineVirtualCamera defaultCam = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        CinemachineVirtualCamera startCam = transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Transform endBlock = GameObject.FindGameObjectWithTag("Finish").transform;
        defaultCam.Follow = player;
        defaultCam.LookAt = player;
        startCam.Follow = endBlock;
        startCam.LookAt = endBlock;
    }

}
