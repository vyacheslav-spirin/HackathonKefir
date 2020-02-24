using UnityEngine;

public class MoveDoor : Door
{
    public Vector3 openOffset;

    public float openSpeed;

    private Vector3 targetPos;
    
    private bool isOpened;

    public override void Open()
    {
        if (isOpened) return;
        
        isOpened = true;

        targetPos = transform.position + openOffset;
    }

    private void Update()
    {
        if (isOpened)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, openSpeed * Time.deltaTime);
        }
    }
}