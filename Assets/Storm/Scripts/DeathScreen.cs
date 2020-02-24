using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Update()
    {
        if (!PlayerController.IsKilled) return;

        spriteRenderer.enabled = true;

        var color = spriteRenderer.color;

        color.a += 3f * Time.deltaTime;
        if (color.a > 1) color.a = 1;

        spriteRenderer.color = color;
    }
}