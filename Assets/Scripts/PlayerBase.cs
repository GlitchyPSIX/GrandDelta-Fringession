using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    internal CharacterController _controller;
    internal Animator anm;

    float collidersep;
    /// <summary>
    /// This is the separation between the base of the Capsule Collider that this player bears
    /// and the ground.
    /// </summary>
    public float ColliderSeparation { get { return collidersep; } set { collidersep = value; } }

    float angl;
    public float Angle { get { return angl; } set { } }
    Quaternion rot;

    Vector3 gravVel;

    float slopeFrc;
    public float SlopeFriction { get { return slopeFrc; } set { slopeFrc = value; } }

    float slopeSpd;
    public float SlopeSlideSpeed { get { return slopeSpd; } set { slopeSpd = value; } }

    float spd;
    public float Speed { get { return spd; } set { spd = value; } }

    bool acceptableSlope;
    bool grounded;
    bool staircased;

    public float _raycastDistance;
    RaycastHit rayhit;
    RaycastHit rayhit_s;

    float horz;
    float vert;

    float pureHorz;
    float pureVert;

    float jheight;
    public float JumpHeight { get { return jheight; } set { jheight = value; } }

    internal Vector3 hitNormal;
    internal Vector3 move;
    internal Vector3 unfilteredMove;

    public bool onGround()
    {
        /* We determine if the character is on ground by checking if
         1, the capsule cast below collides with *something*,
         2, the angle of the slope is within the acceptable margin
         In case we were not on a slope and we're, indeed, floating somewhere
         we slide down said slope later in the code.
        */

        return grounded;

        /* return (Vector3.Angle(Vector3.up, hitNormal) <= _controller.slopeLimit) && Physics.CheckCapsule(GetComponent<Collider>().bounds.center,
            new Vector3(GetComponent<Collider>().bounds.center.x,
            GetComponent<Collider>().bounds.min.y - collidersep, GetComponent<Collider>().bounds.center.z),
             0.72f); */
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Determine the direction the slope is going to
        hitNormal = hit.normal;
        acceptableSlope = (Vector3.Angle(Vector3.up, hitNormal) <= _controller.slopeLimit);
    }

    public void UpdateAxes()
    {
        Debug.DrawRay(GetComponent<Collider>().bounds.center, Vector3.down * _raycastDistance);
        Debug.DrawRay(GetComponent<Collider>().bounds.center, Vector3.down * (_raycastDistance * 1.5f), Color.black);
        horz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        pureHorz = Input.GetAxisRaw("Horizontal");
        pureVert = Input.GetAxisRaw("Vertical");

        Vector3 movez = horz * Camera.main.transform.right;
        Vector3 movex = vert * Camera.main.transform.forward;

        Vector3 moveZ_pure = pureHorz * Camera.main.transform.right;
        Vector3 moveX_pure = pureVert * Camera.main.transform.forward;

        move = movez + movex;
        //We don't really want to adjust the Y value as that's gonna be governed by physics
        move = new Vector3(move.x, 0, move.z);

        /* variable move uses GetAxis, which is smoothed out in case of using keys
           unfilteredMove is essentially this but without the smoothing GetAxis has
           We use this to immediatly check the amount of power the stick is held by so
           we rotate at the correct speed
        */
        unfilteredMove = moveZ_pure + moveX_pure;

        //Angle which we're pointing at
        angl = Mathf.Rad2Deg * Mathf.Atan2(move.x, move.z);
        rot = Quaternion.Euler(0, angl, 0);

        
    }

    public void UpdateAnimatorParameters()
    {
        //Set Animator parameters
        anm.SetFloat("Speed", Vector3.ClampMagnitude(move, 1).magnitude);
        anm.SetBool("isGrounded", onGround() || staircased);
        anm.SetFloat("vertMomentum", gravVel.y);
    }

    public void UpdatePhysics()
    {
        //here it is
        gravVel.y += Physics.gravity.y * 2 * Time.deltaTime;

        //Restore gravity if grounded
        if (onGround() && gravVel.y < 0)
        {
            if (staircased)
            {
            gravVel.y = 0;
            }
            
        }

        //Gravity goes before any other movement
        _controller.Move(gravVel * Time.deltaTime);
    }

    public void InputHandle()
    {
        //Control Jumping
        if (Input.GetButtonDown("Jump") && onGround())
        {
            gravVel.y = (jheight * Physics.gravity.y * -2);
        }

        //If we're touching any axis
        if (unfilteredMove.magnitude != 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Vector3.ClampMagnitude(unfilteredMove, 1).magnitude * 13);
            anm.SetBool("isWalking", true);
        }
        else
        {
            anm.SetBool("isWalking", false);
        }
    }

    public void Move()
    {
        /* Here's what happens if we're not grounded: our movement will be governed by the slope we're
        supposedly standing on
        if (!onGround() && !acceptableSlope)
        {
            move.x += (1f - hitNormal.y) * hitNormal.x * (slopeSpd - slopeFrc);
            move.z += (1f - hitNormal.y) * hitNormal.z * (slopeSpd - slopeFrc);
        }
        Disabling this for now
        I really don't want to deal with this and so far it will not be necessary for me.
        */

        //Now we move
        _controller.Move(move * Time.deltaTime * spd);
        if (Physics.Raycast(GetComponent<Collider>().bounds.center, Vector3.down * _raycastDistance, out rayhit, _raycastDistance))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (Physics.Raycast(GetComponent<Collider>().bounds.center, Vector3.down * (_raycastDistance * 1.5f), out rayhit_s, _raycastDistance * 1.5f))
        {
            staircased = true;
        }
        else
        {
            staircased = false;
        }
    }
}
