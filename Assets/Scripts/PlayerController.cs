using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : PlayerBase
{
    bool jumping;

    public Vector3 drag;

    public Text debugt;
    

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        anm = GetComponent<Animator>();
        Speed = 10;
        SlopeFriction = 2;
        SlopeSlideSpeed = 6;
        ColliderSeparation = 0;
        JumpHeight = 0.8f;
    }

    

    private void Update()
    {
        
        //Debug.DrawLine(GetComponent<Collider>().bounds.center, new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.min.y - collidersep, GetComponent<Collider>().bounds.center.z), Color.white, 1);
        UpdateAxes();
        InputHandle();
        UpdateAnimatorParameters();
        Move();
        UpdatePhysics();
        //debugt.text = Grounded.ToString();
    }
}
