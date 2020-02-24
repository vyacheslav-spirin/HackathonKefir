using UnityEngine;

public class MatMover : MonoBehaviour
{
    public Material mat;

    public string offsetParamName;
    
    public Vector2 offsetVelocity;
    
    void Update()
    {
        mat.SetTextureOffset(offsetParamName, mat.GetTextureOffset(offsetParamName) + offsetVelocity * Time.deltaTime);
    }
}