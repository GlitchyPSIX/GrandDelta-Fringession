using System.Collections;
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
        MaxHP = 4;
        _controller = GetComponent<CharacterController>();
        anm = GetComponent<Animator>();
        BaseSpeed = 10;
        MaxSpeed = 30;
        SlopeFriction = 2;
        SlopeSlideSpeed = 6;
        ColliderSeparation = 0;
        JumpHeight = 0.8f;
        Heal(5);
        spawnpoint = transform.position;
    }

    override public void performUpdate()
    {
        base.performUpdate();
    }
}
