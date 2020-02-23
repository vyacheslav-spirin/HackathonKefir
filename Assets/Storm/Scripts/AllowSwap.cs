using UnityEngine;

public class AllowSwap : MonoBehaviour
{
    public Transform targetPoint;

    private void Awake()
    {
        if (targetPoint == null)
        {
            Debug.LogError("SET TARGET POINT!!! " + gameObject.name);
        }
    }
}