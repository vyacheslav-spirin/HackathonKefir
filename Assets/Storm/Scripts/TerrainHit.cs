using UnityEngine;

public static class TerrainHit
{
    private static readonly RaycastHit[] histBuffer = new RaycastHit[100];

    public static bool Get(Vector3 origin, Vector3 dir, out Vector3 point, out float distance)
    {
        var hitCount = Physics.RaycastNonAlloc(new Ray(origin, dir), histBuffer, 20, -1, QueryTriggerInteraction.Ignore);
        
        distance = float.MaxValue;
        point = Vector3.zero;

        var found = false;

        for (var i = 0; i < hitCount; i++)
        {
            var hit = histBuffer[i];

            if (hit.collider.isTrigger) continue;
            if (hit.collider.GetComponent<TerrainHitIgnore>()) continue;
            
            if(distance < hit.distance) continue;
            distance = hit.distance;

            found = true;

            point = hit.point;
        }

        return found;
    }
}