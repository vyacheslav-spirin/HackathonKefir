using UnityEngine;

public class CharacterActivator : MonoBehaviour
{
    public int id;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController.AddCharacter(id);
    }
}