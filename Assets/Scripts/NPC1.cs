using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC1 : PlayerBase
{
    

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        anm = GetComponent<Animator>();
        BaseSpeed = 10;
        MaxSpeed = 30;
        SlopeFriction = 2;
        SlopeSlideSpeed = 6;
        ColliderSeparation = 0;
        JumpHeight = 0.8f;
    }

    public override void performUpdate()
    {
        UpdateAxes();
        InputHandle();
        UpdatePhysics();
        Move();
    }

    public override void Move()
    {
        // Don't
    }

    public override ActionStates ReturnActionState()
    {
        return ActionStates.TALK;
    }

    public override void Interact()
    {
        Debug.Log("Ding");
    }
}
