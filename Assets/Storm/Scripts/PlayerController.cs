using System;
using System.Collections.Generic;
using Speech;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour , IAnim, ISpeaker
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

    public Animator[] animators;

    //private BlockPos ignoredBlocksCollisionMinPos;
    //private BlockPos ignoredBlocksCollisionMaxPos;

    public CharacterAction currentAction = CharacterAction.No;
    private CharacterAction prevAction = CharacterAction.No;
    public float actionStartTime;


    public CharacterSelector characterSelector;
    public int orderOffset;
    
    
    private float actionCooldownStartTime;


    private int lookDirection;
    private int lastMoveDirection;

    private bool requireJump;

    private bool isGround;

    private float lastJumpTime = -JumpCooldown;
    
    private float horVelocity;
    
    private readonly AnimData[] _animData = new AnimData[100 * 20];
    private int _animDataPos;

    private int charId = 0;

    public int ActiveCharsCount { get; private set; } = 1;
    
    
    
    //skills

    private bool isKilled;

    public static readonly float[] CooldownsTotal =
    {
        4.5f, //Inviser
        5f,   //Hatman
        5f    //Swapper
    };

    public const float InviseTime = 2f;
    
    private readonly float[] _cooldowns = new float[3];

    public float CooldownTotal => CooldownsTotal[charId];
    public float CooldownLeft => _cooldowns[charId];


    public float InvisibleTime { get; private set; }


    public bool IsInvisible => InvisibleTime > 0;
    
    
    
    //



    private static PlayerController _player;
    [SerializeField] private Transform _speechTransform;


    public static PlayerController Instance => _player;
    void Awake()
    {
        Time.timeScale = 1;

        createTime = Time.unscaledTime;
        
        _player = this;
        
        for (var i = 0; i < _animData.Length; i++)
        {
            _animData[i] = new AnimData();
        }
        
        //GetBlocksCollisionPos(out ignoredBlocksCollisionMinPos, out ignoredBlocksCollisionMaxPos);

        //IgnoreBlocksCollision(ignoredBlocksCollisionMinPos, ignoredBlocksCollisionMaxPos);
        
        Step();

        charId = 0;
        
        UpdateChar();

        if (SceneManager.GetActiveScene().name != "Game")
        {
            AddCharacter(1);
            AddCharacter(2);
        }
    }

    private void UpdateChar()
    {
        characterSelector.UpdateRenderers(charId);
        
        characterSelector.UpdateOrderOffset(orderOffset);
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
        actorRigidBody.MovePosition(actorRigidBody.position + new Vector3(Time.fixedDeltaTime * horVelocity, 0, 0));

        if (requireJump)
        {
            requireJump = false;

            actorRigidBody.AddForce(Vector3.up * 6, ForceMode.Impulse);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Game");
        }
            
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("level2");
        }
            
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("level3");
        }
        
        if (isKilled) UpdateKills();
        
        Step();
    }
    
    public void Step()
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

        _animDataPos++;
        _animDataPos %= _animData.Length;
        
        _animData[_animDataPos].Clear();


        if (!isKilled)
        {
            UpdateInput();
            
            var newIsGround = Physics.Raycast(rigidBodyTransform.position + new Vector3(0, 0.1f, 0), -Vector3.up, 0.13f, -1, QueryTriggerInteraction.Ignore);

            if(newIsGround && !isGround)
            {
                SetAnimTrigger("Land");
            }
            else if (!newIsGround && isGround)
            {
                SetAnimTrigger("Jump");
            }

            isGround = newIsGround;

            SetAnimBool("IsGround", isGround);

            //var target = Quaternion.Euler(0, lookDirection > 0 ? 90 : 270, 0);
            //visualTransform.rotation = Quaternion.Slerp(visualTransform.rotation, target, Time.deltaTime * 20f);
        
            if(lookDirection != 0) visualTransform.transform.localScale = new Vector3(lookDirection > 0 ? 1 : -1, 1, 1);

            UpdateVelocity();

            UpdateCurrentAction();
        
            UpdateCooldown();
        }
        

        InvisibleTime -= Time.deltaTime;
        if (InvisibleTime < 0 || isKilled) InvisibleTime = 0;
        
        
        
        //visualTransform.position = actorRigidBody.position;


        var isInvisibleEnabled = InvisibleTime > 0;

        if (isKilled) isInvisibleEnabled = false;

        _animData[_animDataPos].pos = transform.position;
        _animData[_animDataPos].rot = transform.rotation;
        _animData[_animDataPos].look = lookDirection;
        _animData[_animDataPos].isInvisible = isInvisibleEnabled;
        
        characterSelector.SetInvisible(isInvisibleEnabled);
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

        SetAnimFloat("Velocity", Mathf.Abs(horVelocity) / MaxMoveSpeed);

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

        SetAnimInt("Action", (int) currentAction);

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

    private float createTime;

    public void Jump()
    {
        if (isGround && Time.time - lastJumpTime > JumpCooldown)
        {
            requireJump = true;

            lastJumpTime = Time.time;
            
            if(Time.unscaledTime - createTime > 0.5f) _player.jumpSound.Play();
        }
    }

    public void Kick()
    {
        if (currentAction != CharacterAction.No) return;

        if (Time.time - actionCooldownStartTime <= KickCooldown && actionCooldownStartTime > 0) return;

        currentAction = CharacterAction.Kick;

        //actionStartTime = actorManager.matchManager.MatchTime;
    }
    
    private void UpdateInput()
    {
        int dir;
        
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

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
        {
            charId++;

            charId %= ActiveCharsCount;
            
            UpdateChar();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            UseSkill();
        }
    }

    private void UpdateCooldown()
    {
        for (var i = 0; i < _cooldowns.Length; i++)
        {
            ref var cd = ref _cooldowns[i];

            cd -= Time.deltaTime;
            if (cd < 0) cd = 0;
        }
    }


    private void UseSkill()
    {
        ref var cd = ref _cooldowns[charId];

        if (cd > 0) return;
        
        if (charId == 0) //Inviser
        {
            InvisibleTime = InviseTime;
        }
        else if (charId == 1) //Hatman
        {
            var srcPos = transform.position + new Vector3(0, 0.43f, 0);
            
            var ray = LevelCam.Cam.ScreenPointToRay(Input.mousePosition);

            var hPlane = new Plane(new Vector3(0, 0, -1), new Vector3(0, 0, 0));

            if (hPlane.Raycast(ray, out var distance))
            {
                var worldPos3d = ray.GetPoint(distance);

                var hatmanBulletPrefab = Resources.Load<HatmanBullet>("Skills/HatmanBullet");
            
                var hatmanBullet = Instantiate(hatmanBulletPrefab, srcPos, Quaternion.identity);
                hatmanBullet.SetTargetPoint(worldPos3d);

                SetAnimTrigger("Jump");
                
                hatThrowSound.Play();
            }
            else
            {
                return; //skip skill
            }
        } 
        else if (charId == 2) //Swapman
        {
            var srcPos = transform.position + new Vector3(0, 0.43f, 0);
            
            var playerScreenPos3d = LevelCam.Cam.WorldToScreenPoint(srcPos);
            var cursorScreenPos3d = Input.mousePosition;
            
            var playerScreenPos = new Vector2(playerScreenPos3d.x, playerScreenPos3d.y);
            var cursorScreenPos = new Vector2(cursorScreenPos3d.x, cursorScreenPos3d.y);
            
            
            
            var dir = cursorScreenPos - playerScreenPos;
            dir.Normalize();
            if(dir == Vector2.zero) dir = new Vector2(1, 0);
            
            var swapBallPrefab = Resources.Load<SwapBall>("Skills/SwapBall");
            
            var swapBall = Instantiate(swapBallPrefab, srcPos, Quaternion.identity);
            swapBall.SetDir(dir);
        }
        
        cd = CooldownsTotal[charId];
    }
    
    
    
    private void SetAnimTrigger(string triggerName)
    {
        foreach (var animator in animators)
        {
            animator.SetTrigger(triggerName);
        }
        
        _animData[_animDataPos].animTriggers.Add(triggerName);
    }

    private void SetAnimBool(string boolName, bool value)
    {
        foreach (var animator in animators)
        {
             animator.SetBool(boolName, value);
        }
        
        _animData[_animDataPos].animBools.Add(new Tuple<string, bool>(boolName, value));
    }
    
    private void SetAnimFloat(string floatName, float value)
    {
        foreach (var animator in animators)
        {
            animator.SetFloat(floatName, value);
        }
        
        _animData[_animDataPos].animFloats.Add(new Tuple<string, float>(floatName, value));
    }
    
    private void SetAnimInt(string intName, int value)
    {
        foreach (var animator in animators)
        {
            animator.SetInteger(intName, value);
        }

        _animData[_animDataPos].animInts.Add(new Tuple<string, int>(intName, value));
    }

    public AnimData GetAnimData() => _animData[_animDataPos];

    public AudioSource deathSound;
    public AudioSource jumpSound;
    public AudioSource hatThrowSound;
    
    public int GetCharId()
    {
        return charId;
    }
    
    public static void Kill()
    {
        if (_player.isKilled) return;
        
        _player.isKilled = true;
        
        _player.deathSound.Play();

        _player.actorCollider.enabled = false;
        
        _player.actorRigidBody.velocity = Vector3.zero;
        _player.actorRigidBody.AddForce(new Vector3(0, 4, 0), ForceMode.Impulse);
    }

    public static bool IsKilled => _player.isKilled;

    public static bool Invisible => _player.IsInvisible;

    public static bool DisableAttack => Time.time - _player.lastTpTime < 0.3f;

    private float lastTpTime;
    
    private void UpdateKills()
    {
        transform.Rotate(new Vector3(0, 0, 40 * Time.deltaTime));

        if (Time.timeScale > 0.6f) Time.timeScale = 0.6f;
        
        Time.timeScale = Mathf.Max(0f, Time.timeScale - 0.2f * Time.unscaledDeltaTime);
    }

    public void SetLastTpTime(float tpTime)
    {
        lastTpTime = tpTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<DeathCollision>() != null)
        {
            Kill();
        }
    }

    public static void FailHatmanSkill()
    {
        _player._cooldowns[1] = 0.5f;
    }

    private Follower firstFollower;

    public static void AddCharacter(int id)
    {
        if (_player.ActiveCharsCount == 1 && id == 1) //Active hatman
        {
            _player.ActiveCharsCount = 2;

            _player.firstFollower = Instantiate(Resources.Load<Follower>("Characters/Follower"), _player.transform.position, Quaternion.identity);
            _player.firstFollower.Init(_player.gameObject, _player.orderOffset - 10);
            _player.GetComponent<SpeechController>().AddSpeaker(_player.firstFollower);
        }
        
        if (_player.ActiveCharsCount == 2 && id == 2) //Active swapper
        {
            _player.ActiveCharsCount = 3;
            
            Follower secondFollower = Instantiate(Resources.Load<Follower>("Characters/Follower"), _player.transform.position, Quaternion.identity);
            secondFollower.Init(_player.firstFollower.gameObject, _player.orderOffset - 20);
            _player.GetComponent<SpeechController>().AddSpeaker(secondFollower);
        }
    }

    private bool key1Found;
    
    public static void AddKey(int index)
    {
        if (index == 0)
        {
            _player.key1Found = true;
        }
    }

    public static bool IsContaineKey(int index)
    {
        if (index == 0) return _player.key1Found;

        return false;
    }

    public static int CurCharCount => _player.ActiveCharsCount;
    public static bool IsHatSkillReady => _player._cooldowns[1] < 0.001f;
    int ISpeaker.CharId => GetCharId();
    Transform ISpeaker.SpeechTransform => _speechTransform;
}