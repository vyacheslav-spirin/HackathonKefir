using UnityEngine;

public class CharHatView : MonoBehaviour
{
    public Renderer hatRenderer;

    public Renderer any;
    
    private void Update()
    {
        hatRenderer.enabled = any.enabled && PlayerController.IsHatSkillReady;
    }
}