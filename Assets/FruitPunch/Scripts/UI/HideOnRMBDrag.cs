using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnRMBDrag : MonoBehaviour
{
    private void OnEnable()
    {
        CameraRotation.RightMouseDragEvent += OnRightMouseDragEvent;
    }

    private void OnDisable()
    {
        CameraRotation.RightMouseDragEvent -= OnRightMouseDragEvent;
    }

    private void OnRightMouseDragEvent()
    {
        gameObject.SetActive(false);
    }
}
