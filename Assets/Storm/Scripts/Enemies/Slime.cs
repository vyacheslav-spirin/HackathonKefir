using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Slime : MonoBehaviour, ISwapReceiver
{
    public Animator animator;

    public Transform attackTransform;

    public ParticleSystem particles;

    public Collider coll;
    
    private float idleDelay;
    private bool nextIdleAnim;

    private bool isMove = false;
    private bool isAttack = false;

    private PlayerTarget[] targets;
    
    private void Awake()
    {
        targets = FindObjectsOfType<PlayerTarget>();
    }

    void Update()
    {
        var pos1 = new Vector2(attackTransform.position.x, attackTransform.position.y);

        if (!isAttack && !PlayerController.Invisible && !PlayerController.DisableAttack)
        {
            isAttack = false;
        
            foreach (var target in targets)
            {
                var pos2 = new Vector2(target.transform.position.x, target.transform.position.y);

                if (Mathf.Abs(pos1.x - pos2.x) < 0.6f && Vector2.Distance(pos1, pos2) <= 0.8f)
                {
                    isAttack = true;
                    
                    particles.Play();
                    
                    PlayerController.Kill();
                    
                    break;
                }
            }
        }

        animator.SetBool("Attack", isAttack);

        if (!isAttack)
        {
            if (isMove)
            {
                idleDelay = Random.Range(0.5f, 2f);
            
                animator.SetInteger("Idle", 0);
            }
            else
            {
                idleDelay -= Time.deltaTime;

                if (idleDelay < 0)
                {
                    idleDelay = Random.Range(0.7f, 2.5f);
                
                    animator.SetInteger("Idle", nextIdleAnim ? 2 : 1);

                    nextIdleAnim = !nextIdleAnim;
                }
            }
        }
        
        
    }
    
    
    
    public void Swapped()
    {
        var upSuccess = TerrainHit.Get(transform.position, new Vector3(0, 1, 0), coll, out var upPoint, out var upDistance);
        var downSuccess = TerrainHit.Get(transform.position, new Vector3(0, -1, 0), coll, out var downPoint, out var downDistance);

        if (!upSuccess && !downSuccess)
        {
            Destroy(gameObject);

            return;
        }

        if (upDistance < downDistance)
        {
            transform.position = upPoint;
            
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            transform.position = downPoint;
            
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}