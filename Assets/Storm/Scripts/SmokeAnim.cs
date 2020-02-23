using UnityEngine;

public class SmokeAnim : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public float frameDelay = 0.04f;
    
    private Sprite[] frames;

    private float time;

    private void Awake()
    {
        frames = Resources.LoadAll<Sprite>("Smoke");
    }

    void Update()
    {
        time += Time.deltaTime;

        var frame = (int) (time / frameDelay);

        if (frame >= frames.Length)
        {
            Destroy(gameObject);
        }
        else
        {
            spriteRenderer.sprite = frames[frame];
        }
    }
}