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
        
        var dir = new Vector2(targetPoint.x, targetPoint.y) - new Vector2(transform.position.x, transform.position.y);
        dir.Normalize();

        if (dir != Vector2.zero)
        {
            var rad = Mathf.Atan2(dir.y, dir.x);
            
            if(rad > Mathf.PI / 2) transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * rad);
        }
    }

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private bool successSpawned;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, 6f * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < 0.005f)
        {
            TryPlaceObject();
            
            if(!successSpawned) PlayerController.FailHatmanSkill();
            Destroy(gameObject);
        }
        else
        {
            var size = Physics.OverlapSphereNonAlloc(transform.position, col.radius, collisions);

            for (var i = 0; i < size; i++)
            {
                var c = collisions[i];
                if (c == col) continue;
                if (c.isTrigger) continue;
                if (c == _player.GetComponent<Collider>()) continue;

                if (c.GetComponent<TerrainHitIgnore>() != null || c.GetComponent<Goblin>() != null)
                {
                    if(!successSpawned) PlayerController.FailHatmanSkill();
                    Destroy(gameObject);

                    return;
                }
            }

            for (var i = 0; i < size; i++)
            {
                var c = collisions[i];
                if (c == col) continue;
                if (c.isTrigger) continue;
                if (c == _player.GetComponent<Collider>()) continue;

                TryPlaceObject();

                if(!successSpawned) PlayerController.FailHatmanSkill();
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
            if (c.isTrigger) continue;
            if (c.GetComponent<Goblin>() != null) return;
            if (c == _player.GetComponent<Collider>()) return;
        }

        Instantiate(hatmanBoxPrefab, transform.position, Quaternion.identity);

        successSpawned = true;
    }
}