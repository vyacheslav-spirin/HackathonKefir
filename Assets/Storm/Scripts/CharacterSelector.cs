using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] roots;

    private List<SpriteRenderer>[] groups;

    private int prevIndex = -1;

    private Material mat;


    private PlayerController player;
    

    void Awake()
    {
        mat = new Material(Shader.Find("Custom/Invisible"));
        
        groups = new List<SpriteRenderer>[roots.Length];

        player = FindObjectOfType<PlayerController>();
        if (player == null) throw new Exception("Need a player!");

        for (var i = 0; i < groups.Length; i++)
        {
            var renderers = new List<SpriteRenderer>();
            
            FindRenderers(roots[i].transform, renderers);

            groups[i] = renderers;
        }
    }

    void OnDestroy()
    {
        if(mat != null) Destroy(mat);
    }

    private void FindRenderers(Transform root, List<SpriteRenderer> renderes)
    {
        var childCount = root.childCount;

        for (var i = 0; i < childCount; i++)
        {
            FindRenderers(root.GetChild(i), renderes);
        }

        var rs = root.GetComponents<SpriteRenderer>();

        foreach (var r in rs)
        {
            r.material = mat;
        }
        
        renderes.AddRange(rs);
    }
    
    public void UpdateRenderers(int index)
    {
        if (index == prevIndex) return;

        prevIndex = index;
        
        for (var i = 0; i < groups.Length; i++)
        {
            var renderers = groups[i];

            foreach (var r in renderers)
            {
                r.enabled = i == index;
            }
        }
    }

    public void UpdateOrderOffset(int offset)
    {
        foreach (var rs in groups)
        {
            foreach (var r in rs)
            {
                r.sortingOrder = r.sortingOrder + offset;
            }
        }
    }

    
    private static readonly Color normalColor = new Color(1, 1, 1, 1);
    private static readonly Color invisibleColor = new Color(0.5f, 0.8f, 1, 0.4f);

    private bool isInvisibleEnabled;
    
    private float invisibleProgress;

    void Update()
    {
        if (isInvisibleEnabled)
        {
            invisibleProgress += 4.5f * Time.deltaTime;
        }
        else
        {
            invisibleProgress -= 6f * Time.deltaTime;
        }
        
        invisibleProgress = Mathf.Clamp01(invisibleProgress);

        mat.color = Color.LerpUnclamped(normalColor, invisibleColor, invisibleProgress);
    }

    public void SetInvisible(bool isEnabled)
    {
        isInvisibleEnabled = isEnabled;
    }
}