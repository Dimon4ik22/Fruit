using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using HyperCasualPack;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    public CinemachineVirtualCamera cvc;
    public GameObject JoyStickDetector;
    public Transform basePlayer;
    public void MoveTowardsTargetAndBack(Transform transform)
    {
        cvc.Follow = transform;
        JoyStickDetector.gameObject.SetActive(false);
        basePlayer.GetComponent<Rigidbody>().isKinematic = true;

        Run.After(2, () =>
        {
            basePlayer.GetComponent<Rigidbody>().isKinematic = false;
            cvc.Follow = basePlayer;
            JoyStickDetector.gameObject.SetActive(true);
        });
    }
}
