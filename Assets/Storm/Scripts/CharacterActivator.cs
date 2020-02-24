using UnityEngine;

public class CharacterActivator : MonoBehaviour
{
    public int id;

    private void OnTriggerEnter(Collider other)
    {
        if (other == PlayerController.Instance.actorCollider)
        {
            PlayerController.AddCharacter(id);
        }
    }
}