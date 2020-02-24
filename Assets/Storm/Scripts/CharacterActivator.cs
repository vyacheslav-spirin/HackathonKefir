using UnityEngine;

public class CharacterActivator : MonoBehaviour
{
    public int id;

    public GameObject charObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other == PlayerController.Instance.actorCollider)
        {
            PlayerController.AddCharacter(id);
            
            charObject.SetActive(false);
        }
    }
}