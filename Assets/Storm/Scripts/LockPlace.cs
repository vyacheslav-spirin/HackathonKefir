using UnityEngine;

public class LockPlace : MonoBehaviour
{
    public Renderer[] lockRenderers;
    
    public int keyIndex;

    public Door door;
    
    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        
        if (other == PlayerController.Instance.actorCollider && PlayerController.IsContaineKey(keyIndex))
        {
            used = true;

            foreach (var lockRenderer in lockRenderers)
            {
                lockRenderer.material.color = new Color32(31, 255, 42, 255);
            }

            door.Open();
        }
    }
}