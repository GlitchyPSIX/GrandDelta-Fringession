﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FringeController : PlayerBase
{
    bool jumping;

    public Vector3 drag;

    public Text debugt;
    

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

    override public void performUpdate()
    {
        base.performUpdate();
        debugt.text = Speed.ToString();
    }
}