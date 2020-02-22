using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAnim
{
    AnimData GetAnimData();
}

public class AnimData
{
    public Vector3 pos;
    public Quaternion rot;
        
    public List<Tuple<string, bool>> animBools;
    public List<string> animTriggers;
    public List<Tuple<string, float>> animFloats;
    public List<Tuple<string, int>> animInts;

    public AnimData()
    {
        animBools = new List<Tuple<string, bool>>(10);
        animTriggers = new List<string>(10);
        animFloats = new List<Tuple<string, float>>(10);
        animInts = new List<Tuple<string, int>>(10);
    }

    public void Clear()
    {
        animBools.Clear();
        animTriggers.Clear();
        animFloats.Clear();
        animInts.Clear();
    }
}