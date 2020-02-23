using UnityEngine;

public class HatLife : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 5f);
    }
}