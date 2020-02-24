using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderOffset : MonoBehaviour
{
    public Transform renderersRoot;

    public int orderOffset;
    
    private readonly List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    
    void Awake()
    {
        FindRenderers(renderersRoot);
        
        UpdateOrderOffset(orderOffset);
    }

    private void FindRenderers(Transform root)
    {
        var childCount = root.childCount;

        for (var i = 0; i < childCount; i++)
        {
            FindRenderers(root.GetChild(i));
        }

        var rs = root.GetComponents<SpriteRenderer>();
        
        renderers.AddRange(rs);
    }

    public void UpdateOrderOffset(int offset)
    {
        foreach (var r in renderers)
        {
            r.sortingOrder = r.sortingOrder + offset;
        }
    }
}