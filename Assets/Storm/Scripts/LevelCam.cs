using System;
using UnityEngine;

public class LevelCam : MonoBehaviour
{
    public static Camera Cam { get; private set; }

    private void Awake()
    {
        Cam = GetComponent<Camera>();
    }

    private void OnDestroy()
    {
        if (Cam == GetComponent<Camera>()) Cam = null;
    }
}