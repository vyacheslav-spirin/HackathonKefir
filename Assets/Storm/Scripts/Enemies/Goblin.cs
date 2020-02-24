using UnityEngine;

public class Goblin : MonoBehaviour, ISwapReceiver
{
    public Animator animator;

    public Transform attackTransform;
    
    private float idleTime;

    private int lastIdleAnim;

    private bool isKilled;

    private bool isAttack;
    
    private PlayerTarget[] targets;

    private void Awake()
    {
        targets = FindObjectsOfType<PlayerTarget>();
        
        idleTime = Random.Range(0.5f, 4.5f);
        
        lastIdleAnim = Random.Range(0, 3);
        
        animator.SetInteger("Idle", lastIdleAnim);
    }

    private void Update()
    {
        if (isKilled) return;
        
        if (!isAttack && !PlayerController.Invisible && !PlayerController.DisableAttack)
        {
            isAttack = false;
            
            var pos1 = new Vector2(attackTransform.position.x, attackTransform.position.y);
        
            foreach (var target in targets)
            {
                var pos2 = new Vector2(target.transform.position.x, target.transform.position.y);

                if (Mathf.Abs(pos1.x - pos2.x) < 0.6f && Vector2.Distance(pos1, pos2) <= 0.8f)
                {
                    isAttack = true;

                    idleTime = 100;
                    
                    animator.SetTrigger("Attack");
                    animator.SetInteger("Idle", 10);

                    PlayerController.Kill();
                    
                    break;
                }
            }
        }
        
        
        
        
        idleTime -= Time.deltaTime;
        
        if (idleTime <= 0)
        {
            idleTime = Random.Range(0.5f, 4.5f);

            int anim;
            do
            {
                anim = Random.Range(0, 3);
            } while (anim == lastIdleAnim);

            lastIdleAnim = anim;

            animator.SetInteger("Idle", lastIdleAnim);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<DeathCollision>() != null)
        {
            isKilled = true;
            
            animator.SetBool("Dead", true);
        }
    }

    public void Swapped()
    {
        if (isKilled) return;
        
        idleTime = 3;

        lastIdleAnim = Random.Range(3, 6);
        
        animator.SetInteger("Idle", lastIdleAnim);
    }
}