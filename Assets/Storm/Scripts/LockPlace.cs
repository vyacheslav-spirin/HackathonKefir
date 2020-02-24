using UnityEngine;

public class LockPlace : MonoBehaviour
{
    public Renderer lockRenderer;
    
    public int keyIndex;

    public Door door;
    
    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        
        if (other == PlayerController.Instance.actorCollider)
        {
            used = true;
            
            lockRenderer.material.color = new Color32(31, 255, 42, 255);
            
            door.Open();
        }
    }
}