using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCPI : MonoBehaviour
{
    public Canvas myCanvas;
    Animator anim;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = myCanvas.transform.TransformPoint(pos);

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Active");
        }
    }
}
