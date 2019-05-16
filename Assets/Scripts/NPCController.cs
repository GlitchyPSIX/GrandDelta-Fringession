using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : PlayerBase
{
    // Start is called before the first frame update
    void Start()
    {
        NPC = true;
        _controller = GetComponent<CharacterController>();
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
        // UpdateAnimatorParameters();
        UpdatePhysics();
        Move();
        checkInteraction();
    }
}
