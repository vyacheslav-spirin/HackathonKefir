using UnityEngine;

public class KeyPlace : MonoBehaviour
{
    public GameObject key;

    public int keyIndex;
    
    private bool used;
    
    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        
        if (other == PlayerController.Instance.actorCollider)
        {
            used = true;
            
            key.SetActive(false);
            
            PlayerController.AddKey(keyIndex);
        }
    }
}