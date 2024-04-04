using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Camera orthoCamera;

    private void Start()
    {
        // Assuming you have a reference to your orthographic camera in the scene
        orthoCamera = Camera.main;
    }

    private void LateUpdate()
    {
        // Rotate the canvas to face the orthographic camera
        transform.rotation = Quaternion.LookRotation(orthoCamera.transform.forward, orthoCamera.transform.up);
    }
}
