﻿using UnityEngine;

public class Follower : MonoBehaviour, IAnim
{
    public GameObject parentGo;

    public Animator[] animators;

    public CharacterSelector characterSelector;
    
    public int skinOffset;
    public int orderOffset;
    
    private const int offsetFrames = 12;

    private AnimData[] frames = new AnimData[100 * 10];

    private IAnim parent;
    
    private int readPos;
    private int writePos;


    private int lastCheckFrame = -1;

    void Awake()
    {
        if (parent != null) return;
        
        Init();
        
        characterSelector.UpdateOrderOffset(orderOffset);
    }
    
    private void Init()
    {
        parent = parentGo.GetComponent<IAnim>();
        
        writePos = 0;
        readPos = 0;
        
        for (var i = 0; i < offsetFrames; i++)
        {
            frames[writePos % frames.Length] = parent.GetAnimData();
            writePos++;
        }

        transform.position = parent.GetAnimData().pos;
    }

    void Update()
    {
        Fetch();

        var frame = frames[readPos % frames.Length];
        
        transform.position = frame.pos + new Vector3(0, 0, 0.17f);
        if(frame.look != 0) transform.localScale = new Vector3(frame.look > 0 ? 1 : -1, 1, 1);

        var charId = parent.GetCharId() + skinOffset;
        charId %= 3;

        characterSelector.UpdateRenderers(charId);
        
        foreach (var animBool in frame.animBools)
        {
            foreach (var animator in animators)
            {
                animator.SetBool(animBool.Item1, animBool.Item2);
            }
        }
        
        foreach (var animTrigger in frame.animTriggers)
        {
            foreach (var animator in animators)
            {
                animator.SetTrigger(animTrigger);
            }
        }
        
        foreach (var animFloat in frame.animFloats)
        {
            foreach (var animator in animators)
            {
                animator.SetFloat(animFloat.Item1, animFloat.Item2);
            }
        }
        
        foreach (var animInt in frame.animInts)
        {
            foreach (var animator in animators)
            {
                animator.SetInteger(animInt.Item1, animInt.Item2);
            }
        }
    }

    void LateUpdate()
    {
        readPos++;
    }

    private void Fetch()
    {
        if(parent == null) Init();
        
        if (Time.frameCount != lastCheckFrame)
        {
            lastCheckFrame = Time.frameCount;

            frames[writePos % frames.Length] = parent.GetAnimData();

            writePos++;
        }
    }

    public AnimData GetAnimData()
    {
        Fetch();
        
        return frames[readPos % frames.Length];
    }

    public int GetCharId()
    {
        return parent.GetCharId() + skinOffset;
    }
}