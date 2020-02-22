using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] roots;

    private List<SpriteRenderer>[] groups;

    private int prevIndex = -1;
    
    void Awake()
    {
        groups = new List<SpriteRenderer>[roots.Length];

        for (var i = 0; i < groups.Length; i++)
        {
            var renderers = new List<SpriteRenderer>();
            
            FindRenderers(roots[i].transform, renderers);

            groups[i] = renderers;
        }
    }

    private void FindRenderers(Transform root, List<SpriteRenderer> renderes)
    {
        var childCount = root.childCount;

        for (var i = 0; i < childCount; i++)
        {
            FindRenderers(root.GetChild(i), renderes);
        }

        var rs = root.GetComponents<SpriteRenderer>();
        
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
}