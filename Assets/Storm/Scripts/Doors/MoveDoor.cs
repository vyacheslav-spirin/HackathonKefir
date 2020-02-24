using UnityEngine;

public class MoveDoor : Door
{
    public Vector3 openOffset;

    public float openSpeed;

    public AudioSource sound;

    private Vector3 targetPos;
    
    private bool isOpened;

    public override void Open()
    {
        if (isOpened) return;
        
        isOpened = true;

        targetPos = transform.position + openOffset;
        
        sound.Play();
    }

    private void Update()
    {
        if (isOpened)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, openSpeed * Time.deltaTime);
        }
    }
}