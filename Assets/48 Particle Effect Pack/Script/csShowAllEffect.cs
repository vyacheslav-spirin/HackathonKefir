﻿using UnityEngine;
using System.Collections;

public class csShowAllEffect : MonoBehaviour
{
    public string[] EffectNames;
    public string[] Effect2Names;
    public Transform[] Effect;
    public GUIText Text1;
    int i = 0;
    int a = 0;


    void Start()
    {
        Instantiate(Effect[i], new Vector3(0, 5, 0), Quaternion.identity);
    }


    void Update()
    {
        Text1.text = i + 1 + ":" + EffectNames[i];



    }
}
