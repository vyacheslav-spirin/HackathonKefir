using UnityEngine;

public class KeyPlace : MonoBehaviour
{
    public GameObject key;

    public int keyIndex;

    public AudioSource sound;
    
    private bool used;
    
    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        
        if (other == PlayerController.Instance.actorCollider)
        {
            used = true;
            
            key.SetActive(false);
            
            sound.Play();
            
            PlayerController.AddKey(keyIndex);
        }
    }
}