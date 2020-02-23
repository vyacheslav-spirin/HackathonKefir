using UnityEngine;

public class HatmanBullet : MonoBehaviour
{
    public SphereCollider col;
    
    private Vector3 targetPoint;
    
    private Collider[] collisions = new Collider[100];

    private PlayerController _player;

    public void SetTargetPoint(Vector3 point)
    {
        targetPoint = point;
    }

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, 6f * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < 0.005f)
        {
            TryPlaceObject();
            
            Destroy(gameObject);
        }
        else
        {
            var size = Physics.OverlapSphereNonAlloc(transform.position, col.radius, collisions);

            for (var i = 0; i < size; i++)
            {
                var c = collisions[i];
                if (c == col) continue;
                if (c == _player.GetComponent<Collider>()) continue;

                if (c.GetComponent<TerrainHitIgnore>() != null)
                {
                    Destroy(gameObject);

                    return;
                }
            }
            
            for (var i = 0; i < size; i++)
            {
                var c = collisions[i];
                if (c == col) continue;
                if (c == _player.GetComponent<Collider>()) continue;

                TryPlaceObject();

                Destroy(gameObject);

                return;
            }
        }
    }

    private void TryPlaceObject()
    {
        var hatmanBoxPrefab = Resources.Load<GameObject>("Skills/HatmanBox");
        var prefabCol = hatmanBoxPrefab.GetComponent<BoxCollider>();
        
        var size = Physics.OverlapBoxNonAlloc(transform.position + prefabCol.center, prefabCol.size, collisions, Quaternion.identity);
        
        for (var i = 0; i < size; i++)
        {
            var c = collisions[i];
            if (c == col) continue;
            
            if (c == _player.GetComponent<Collider>()) return;
        }

        Instantiate(hatmanBoxPrefab, transform.position, Quaternion.identity);
    }
}