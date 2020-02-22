using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum CharacterAction
    {
        No,
        Kick
    }

    private const float Deceleration = 30f;
    private const float Acceleration = 12f;
    private const float MaxMoveSpeed = 2.5f;

    private const float JumpCooldown = 0.2f;

    private const float KickTime = 0.25f;
    private const float KickCooldown = 0.7f;
    
    public Collider actorCollider;
    public Rigidbody actorRigidBody;

    public ActorCollisionDetector actorCollisionDetector;

    public Transform visualTransform;
    public Transform rigidBodyTransform;

    private readonly List<Collider> ignoredBlocksCollision = new List<Collider>(6);

    public Animator animator;

    public Vector2 Velocity
    {
        get => actorRigidBody.velocity;
        set => actorRigidBody.velocity = value;
    }

    //private BlockPos ignoredBlocksCollisionMinPos;
    //private BlockPos ignoredBlocksCollisionMaxPos;

    public CharacterAction currentAction = CharacterAction.No;
    private CharacterAction prevAction = CharacterAction.No;
    public float actionStartTime;

    private float actionCooldownStartTime;

    //public for multiplayer actor manager
    public int lookDirection;
    public int lastMoveDirection;

    private bool requireJump;

    private bool isGround;

    private float lastJumpTime = -JumpCooldown;
    
    private float horVelocity;
    
    void Awake()
    {
        //GetBlocksCollisionPos(out ignoredBlocksCollisionMinPos, out ignoredBlocksCollisionMaxPos);

        //IgnoreBlocksCollision(ignoredBlocksCollisionMinPos, ignoredBlocksCollisionMaxPos);
    }

    /*private void IgnoreBlocksCollision(BlockPos min, BlockPos max)
    {
        var mapManager = actorManager.matchManager.mapManager;

        for (var i = min.x; i <= max.x; i++)
        {
            for (var j = min.y; j <= max.y; j++)
            {
                var block = mapManager.GetBlock(i, j);

                if (block != null &&
                    mapManager.GetBlockDescriptionByTypeId(block.typeId).blockCollisionBehaviour == BlockDescription.BlockCollisionBehaviour.TopCollision)
                {
                    ignoredBlocksCollision.Add(new BlockPos(i, j));

                    Physics.IgnoreCollision(actorCollider, block.collider, true);
                }
            }
        }
    }*/

    void FixedUpdate()
    {
        actorRigidBody.MovePosition(rigidBodyTransform.position + new Vector3(Time.fixedDeltaTime * horVelocity, 0, 0));

        if (requireJump)
        {
            requireJump = false;

            actorRigidBody.AddForce(Vector3.up * 6, ForceMode.Impulse);
        }
    }
    
    void Update()
    {
        /*GetBlocksCollisionPos(out var min, out var max);

        if (ignoredBlocksCollisionMinPos != min || ignoredBlocksCollisionMaxPos != max)
        {
            //can optimized, with diff remove and diff add

            var mapManager = actorManager.matchManager.mapManager;

            foreach (var blockPos in ignoredBlocksCollision)
            {
                var block = mapManager.GetBlock(blockPos.x, blockPos.y);

                if(block == null) continue;

                Physics.IgnoreCollision(actorCollider, block.collider, false);
            }
            ignoredBlocksCollision.Clear();

            ignoredBlocksCollisionMinPos = min;
            ignoredBlocksCollisionMaxPos = max;

            IgnoreBlocksCollision(ignoredBlocksCollisionMinPos, ignoredBlocksCollisionMaxPos);
        }*/


        UpdateInput();
        
        
        var newIsGround = Physics.Raycast(rigidBodyTransform.position + new Vector3(0, 0.1f, 0), -Vector3.up, 0.13f);

        if(newIsGround && !isGround)
        {
            animator.SetTrigger("Land");
        }
        else if (!newIsGround && isGround)
        {
            animator.SetTrigger("Jump");
        }

        isGround = newIsGround;

        animator.SetBool("IsGround", isGround);

        var target = Quaternion.Euler(0, lookDirection > 0 ? 90 : 270, 0);
        visualTransform.rotation = Quaternion.Slerp(visualTransform.rotation, target, Time.deltaTime * 20f);

        UpdateVelocity();

        UpdateCurrentAction();
        
        visualTransform.position = actorRigidBody.position;
    }

    private void UpdateVelocity()
    {
        if (lastMoveDirection > 0)
        {
            if (horVelocity < 0)
            {
                horVelocity += Time.deltaTime * Deceleration;

                if (horVelocity > 0) horVelocity = 0;
            }
            else
            {
                horVelocity += Time.deltaTime * Acceleration;

                if (horVelocity > MaxMoveSpeed) horVelocity = MaxMoveSpeed;
            }
        }
        else if (lastMoveDirection < 0)
        {
            if (horVelocity > 0)
            {
                horVelocity -= Time.deltaTime * Deceleration;

                if (horVelocity < 0) horVelocity = 0;
            }
            else
            {
                horVelocity -= Time.deltaTime * Acceleration;

                if (horVelocity < -MaxMoveSpeed) horVelocity = -MaxMoveSpeed;
            }
        }
        else
        {
            if (horVelocity > 0)
            {
                horVelocity -= Time.deltaTime * Deceleration;

                if (horVelocity < 0) horVelocity = 0;
            }
            else if (horVelocity < 0)
            {
                horVelocity += Time.deltaTime * Deceleration;

                if (horVelocity > 0) horVelocity = 0;
            }
        }

        animator.SetFloat("Velocity", Mathf.Abs(horVelocity) / MaxMoveSpeed);

        lastMoveDirection = 0;
    }

    private void UpdateCurrentAction()
    {
        /*if (currentAction == CharacterAction.Kick)
        {
            if (actorManager.matchManager.MatchTime - actionStartTime > KickTime)
            {
                currentAction = CharacterAction.No;

                actionCooldownStartTime = Time.time;
            }
        }*/

        animator.SetInteger("Action", (int) currentAction);

        if (prevAction != currentAction)
        {
            if (prevAction == CharacterAction.Kick)
            {
                OnKick();
            }
        }

        prevAction = currentAction;
    }

    private void OnKick()
    {
        //actorManager.matchManager.gameModeManager.ProcessKickAction(this);
    }


    //Controls

    public void Move(int direction)
    {
        if(currentAction == CharacterAction.No) lookDirection = direction;

        lastMoveDirection = direction;
    }

    public void Jump()
    {
        if (isGround && Time.time - lastJumpTime > JumpCooldown)
        {
            requireJump = true;

            lastJumpTime = Time.time;
        }
    }

    public void Kick()
    {
        if (currentAction != CharacterAction.No) return;

        if (Time.time - actionCooldownStartTime <= KickCooldown && actionCooldownStartTime > 0) return;

        currentAction = CharacterAction.Kick;

        //actionStartTime = actorManager.matchManager.MatchTime;
    }

    private int lastDir;

    private void UpdateInput()
    {
        int dir = 0;
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            dir = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            dir = 1;
        }
        else
        {
            dir = 0;
        }

        Move(dir);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
}