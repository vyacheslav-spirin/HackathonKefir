using UnityEngine;

public class CageHeadSwitcher : MonoBehaviour
{
    public GameObject[] normalHead;

    public GameObject cageHead;


    public void Switch()
    {
        foreach (var o in normalHead)
        {
            o.SetActive(false);
        }
        
        cageHead.SetActive(true);
    }
}