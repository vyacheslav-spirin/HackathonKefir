using System;
using UnityEngine;

public class SwapBall : MonoBehaviour
{
    private Vector3 dir3d = new Vector3(1, 0, 0);
    
    private float timeLeft = 1.5f;
    
    private Collider[] collisions = new Collider[2];

    private PlayerController _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    public void SetDir(Vector2 dir)
    {
        dir3d = new Vector3(dir.x, dir.y, 0);
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0) Destroy(gameObject);
        else
        {
            transform.position = transform.position + (dir3d * 7 * Time.deltaTime);

            var col = GetComponent<SphereCollider>();

            var size = Physics.OverlapSphereNonAlloc(transform.position, col.radius, collisions);

            for (var i = 0; i < size; i++)
            {
                var c = collisions[i];
                if(c == col) continue;
                if (c == _player.GetComponent<Collider>()) continue;

                
                if (c.GetComponent<AllowSwap>() != null)
                {
                    var playerPos = _player.transform.position;
                    var targetPos = c.transform.position;

                    c.transform.position = playerPos;
                    _player.transform.position = targetPos;
                }

                Destroy(gameObject);

                return;
            }
        }
    }
}