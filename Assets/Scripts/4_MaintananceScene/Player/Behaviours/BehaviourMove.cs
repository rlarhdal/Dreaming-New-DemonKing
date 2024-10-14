using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BehaviourMove : BehaviourInput
{
    protected Rigidbody2D rb2D;
    Vector3 left;
    Vector3 right;
    
    protected PlayerInputHandler handler;

    Animator animator;
    Stat<float> MoveStat;
    Stat<float> plusMoveStat;

    Vector2 moveVector;

    Transform playerCharacterTransform;
    void Start()
    {
//        Init();
    }

    public override void Init()
    {
        playerCharacterTransform = Util.FindChild<Transform>(gameObject, "UnitRoot",true);
        animator = GetComponentInChildren<Animator>();
        MoveStat = Managers.Game.player.StatHandler.GetStat<float>(StatSpecies.Speed);
        plusMoveStat = Managers.Game.player.StatHandler.GetStat<float>(StatSpecies.plusSpeed);
        input = new InputMove();
        stateMachine = Managers.Game.player.StateHandler.stateMachine;
        state = Managers.Game.player.StateHandler.GetState(ActionType.Move);
        rb2D = GetComponent<Rigidbody2D>();
        left = new Vector3(-1, 1, 1);
        right = new Vector3(1, 1, 1);

        handler = Managers.Game.player.InputHandler;
        handler.data[(int)ActionType.Move].Subscribers[InputStatus.Started]   += Started;
        handler.data[(int)ActionType.Move].Subscribers[InputStatus.Performed] += Performed;
        handler.data[(int)ActionType.Move].Subscribers[InputStatus.Canceled]  += Canceled;
        handler.SubscribeToggle();
    }

    protected override void Started(InputAction.CallbackContext context)
    {
        if (stateMachine.ChangeState(state) && Managers.Game.player.canMove)
        {
            animator.SetBool("Move",true);
        }
    }

    protected override void Performed(InputAction.CallbackContext context)
    {
        if (stateMachine.GetCurrentStates(StateLayer.Die)[0] != null || !Managers.Game.player.canMove)
            return;
        // Movement
        float speed = MoveStat.value * (1f + plusMoveStat.value);
        moveVector = speed * context.ReadValue<Vector2>().normalized;
        
        Movement();
    }

    protected override void Canceled(InputAction.CallbackContext context)
    {
        // Stop
        rb2D.velocity = Vector2.zero;
        
        animator.SetBool("Move",false);
        stateMachine.ChangeState(Managers.Game.player.StateHandler.GetState(ActionType.Idle));
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(!other.gameObject.CompareTag("Enemy"))
            Movement();
    }

    // Test for animation event
    void Movement()
    {
        rb2D.velocity = moveVector;
        if (rb2D.velocity.x > 0)
        {
            playerCharacterTransform.localScale = left; // default facing: left, we use Vector3.left for facing right
        }
        else if (rb2D.velocity.x < 0)
        {
            playerCharacterTransform.localScale = right;
        }
    }
}