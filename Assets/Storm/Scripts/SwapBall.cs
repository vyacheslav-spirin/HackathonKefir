using System;
using UnityEngine;

public class SwapBall : MonoBehaviour
{
    private Vector3 dir3d = new Vector3(1, 0, 0);
    
    private float timeLeft = 1.5f;
    
    private Collider[] collisions = new Collider[2];

    private PlayerController _player;
    private Follower[] _followers;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        
        _followers = FindObjectsOfType<Follower>();
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

                var allowSwap = c.GetComponent<AllowSwap>();
                
                if (allowSwap != null)
                {

                    var playerPos = _player.transform.position + new Vector3(0, 0.45f, 0);
                    var targetPos = allowSwap.targetPoint.position;

                    playerPos.z = 0;
                    targetPos.z = 0;

                    c.transform.position = playerPos;
                    _player.transform.position = targetPos;
                    
                    _player.SetLastTpTime(Time.time);
                    
                    _player.Step();
                    foreach (var follower in _followers)
                    {
                        follower.Reset();
                    }

                    c.GetComponent<ISwapReceiver>()?.Swapped();



                    
                    var smokeEffectPrefab = Resources.Load<GameObject>("SmokeEffect");
                    Instantiate(smokeEffectPrefab, playerPos, Quaternion.identity);
                    Instantiate(smokeEffectPrefab, targetPos, Quaternion.identity);
                }

                Destroy(gameObject);

                return;
            }
        }
    }
}