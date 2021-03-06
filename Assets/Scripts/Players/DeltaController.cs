﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltaController : PlayerBase
{
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        BaseSpeed = 10;
        MaxSpeed = 30;
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
