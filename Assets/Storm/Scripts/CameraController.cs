using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform backCameraTransform;

    public Transform forwardCameraTransform;

    private Vector2 lastCamTarget;
    
    public Transform playerCharacterTransform;

    void LateUpdate()
    {
        if (playerCharacterTransform == null) return;
        
        lastCamTarget = playerCharacterTransform.position + new Vector3(0, 1, 0);

        var camPos = forwardCameraTransform.position;
        camPos = Vector3.Lerp(camPos, new Vector3(lastCamTarget.x, lastCamTarget.y, camPos.z), Time.deltaTime * 7f);
        forwardCameraTransform.position = camPos;

        camPos = backCameraTransform.position;
        camPos = Vector3.Lerp(camPos, new Vector3(lastCamTarget.x / 4, lastCamTarget.y / 4, camPos.z), Time.deltaTime * 7f);
        backCameraTransform.position = camPos;
    }

    public static void Look(Transform transform)
    {
        FindObjectOfType<CameraController>().playerCharacterTransform = transform;
    }
}