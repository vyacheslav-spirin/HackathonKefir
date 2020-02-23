using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Slime : MonoBehaviour, ISwapReceiver
{
    public Animator animator;

    public Transform attackTransform;

    public ParticleSystem particles;
    
    private float idleDelay;
    private bool nextIdleAnim;

    private int isMove = 0;
    private Vector3 targetMovePoint;
    private bool isAttack = false;

    private PlayerTarget[] targets;

    private void Awake()
    {
        targets = FindObjectsOfType<PlayerTarget>();
    }

    void Update()
    {
        if (isMove != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetMovePoint, 5 * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetMovePoint) < 0.001f)
            {
                CompleteMove();

                isMove = 0;
            }
        }
        
        
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
            
            if (!isAttack && isMove == 0)
            {
                foreach (var target in targets)
                {
                    var pos2 = new Vector2(target.transform.position.x, target.transform.position.y);

                    var h = pos2.y - pos1.y;
                    
                    if (Mathf.Abs(pos1.x - pos2.x) < 0.6f && Mathf.Abs(h) < 4f)
                    {
                        if (h > 0)
                        {
                            if (TerrainHit.Get(transform.position, new Vector3(0, 1, 0), out targetMovePoint, out _))
                            {
                                isMove = 1;
                                PrepareMove();
                            }
                        }
                        else
                        {
                            if (!TerrainHit.Get(transform.position, new Vector3(0, -1, 0), out targetMovePoint, out _))
                            {
                                targetMovePoint = transform.position + new Vector3(0, -300, 0);
                            }
                            
                            isMove = -1;
                            PrepareMove();
                        }

                        break;
                    }
                }
            }
        }

        animator.SetBool("Attack", isAttack);

        if (!isAttack)
        {
            if (isMove == 0)
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
    
    private void PrepareMove()
    {
        if (isMove == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    private void CompleteMove()
    {
        if (isMove == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    
    public void Swapped()
    {
        isMove = 0;

        var upSuccess = TerrainHit.Get(transform.position, new Vector3(0, 1, 0), out var upPoint, out var upDistance);
        var downSuccess = TerrainHit.Get(transform.position, new Vector3(0, -1, 0), out var downPoint, out var downDistance);

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