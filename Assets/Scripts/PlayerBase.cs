using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    internal CharacterController _controller;
    internal Animator anm;
    private bool _npc = false;

    float collidersep;
    /// <summary>
    /// This is the separation between the base of the Capsule Collider that this player bears
    /// and the ground.
    /// </summary>
    public float ColliderSeparation { get { return collidersep; } set { collidersep = value; } }

    float angl;
    public float Angle { get { return angl; } set { } }
    private Quaternion rot;

    private Vector3 gravVel;

    private float slopeFrc;
    public float SlopeFriction { get { return slopeFrc; } set { slopeFrc = value; } }

    private float slopeSpd;
    public float SlopeSlideSpeed { get { return slopeSpd; } set { slopeSpd = value; } }

    private float currSpd;
    private float spd;
    public float Speed { get { return spd; } private set { } }

    private float baseSpd;
    public float BaseSpeed { get { return baseSpd; } set { baseSpd = value; } }

    private float maxSpd;
    public float MaxSpeed { get { return maxSpd; } set { maxSpd = value; } }

    private bool acceptableSlope;
    private bool grounded;
    private bool staircased;
    private bool walking;

    private float interactionTimer;
    public float interactionLimit = 1f;

    private float poundTimer;
    public float poundTimerLimit = 1f;

    public bool Still { get; set; }

    public float _raycastDistance;
    private RaycastHit rayhit;
    private RaycastHit rayhit_s;
    private RaycastHit rayhit_go;

    private float horz;
    private float vert;

    private float pureHorz;
    private float pureVert;

    float jheight;
    public float JumpHeight { get { return jheight; } set { jheight = value; } }

    public bool NPC { get => _npc; set => _npc = value; }

    public enum PoundStatus
    {
        NOT, PREPARE, FALLING
    }

    PoundStatus poundPhase = PoundStatus.NOT;

    internal Vector3 hitNormal;
    internal Vector3 move;
    internal Vector3 unfilteredMove;

    public Text debugtext;

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

    public virtual void UpdateAxes(bool useExternal = false, float _horz = 0, float _vert = 0)
    {
        
            Debug.DrawRay(GetComponent<Collider>().bounds.center, transform.forward * (_raycastDistance * 1.5f), Color.red);
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

        if (poundPhase == PoundStatus.NOT)
        {
            if (useExternal == false)
            {
                move = movez + movex;
            }
            else
            {
                move = _horz * Vector3.one + _vert * Vector3.one;
            }
            //We don't really want to adjust the Y value as that's gonna be governed by physics
            move = new Vector3(move.x, 0, move.z);

            /* variable move uses GetAxis, which is smoothed out in case of using keys
               unfilteredMove is essentially this but without the smoothing GetAxis has
               We use this to immediatly check the amount of power the stick is held by so
               we rotate at the correct speed
            */

            if (useExternal == false)
            {
                unfilteredMove = moveZ_pure + moveX_pure;
            }
            else
            {
                unfilteredMove = _horz * Vector3.one + _vert * Vector3.one;
            }

            //Angle which we're pointing at
            angl = Mathf.Rad2Deg * Mathf.Atan2(move.x, move.z);
            rot = Quaternion.Euler(0, angl, 0);
        }
        else
        {
            move = Vector3.zero;
            unfilteredMove = Vector3.zero;
        }
    }

    public void UpdateAnimatorParameters()
    {
        //Set Animator parameters
        anm.SetFloat("Speed", Vector3.ClampMagnitude(move, 1).magnitude * (spd/10));
        anm.SetBool("isGrounded", onGround() || staircased);
        anm.SetFloat("vertMomentum", gravVel.y);
        anm.SetBool("isWalking", walking);
    }

    public void UpdatePhysics()
    {
        //here it is
        gravVel.y += Physics.gravity.y * 2 * Time.deltaTime;

        //Restore gravity if grounded
        if (grounded && gravVel.y < 0)
        {
            gravVel.y = 0;         
        }

        //Gravity goes before any other movement
        if (poundPhase != PoundStatus.PREPARE)
        {
            _controller.Move(gravVel * Time.deltaTime);
        }
        else
        {
            gravVel.y += Physics.gravity.y * 8 * Time.deltaTime;
        }
    }

    public void Jump()
    {
        gravVel.y = (jheight * Physics.gravity.y * -2);
    }

    public void InputHandle()
    {
        if (!_npc)
        {
            //Control Jumping
            if (Input.GetButtonDown("Jump") && (onGround() || staircased))
            {
                Jump();
            }

            if (Input.GetButton("Run") && (onGround() || staircased))
            {
                spd = Mathf.SmoothDamp(spd, maxSpd, ref currSpd, 0.1f);
            }
            else if (!Input.GetButton("Run") && !(onGround() || staircased))
            {
                spd = Mathf.SmoothDamp(spd, baseSpd, ref currSpd, 2f);
            }
            else if (!Input.GetButton("Run"))
            {
                spd = Mathf.SmoothDamp(spd, baseSpd, ref currSpd, 0.1f);
            }

            if (Input.GetButtonDown("Pound") && !(onGround() || staircased) && poundPhase == PoundStatus.NOT)
            {
                anm.SetBool("isPounding", true);
                move = Vector3.zero;
                gravVel = Vector3.zero;
                anm.Play("Groundpound");
                poundPhase = PoundStatus.PREPARE;
                poundTimer = 0;
            }

            if (Input.GetButtonDown("Interact") && (onGround() || staircased))
            {
                GameObject interactable = checkInteraction();
                if (interactable != null)
                {
                    interactable.GetComponent<InteractableNPC>().Action();
                }
            }

            //If we're touching any axis
            if (unfilteredMove.magnitude != 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Vector3.ClampMagnitude(unfilteredMove, 1).magnitude * 13);
                walking = true;
            }
            else
            {
                walking = false;
            }
        }
    }

    public virtual void Move()
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
        if (poundPhase != PoundStatus.PREPARE)
        {
            _controller.Move(move * Time.deltaTime * spd);
        }
        checkGrounded();
    }

    private void Update()
    {
        debugtext.text = poundPhase.ToString() + "\n" + poundTimer + "\n" + interactionTimer.ToString();
        if (interactionTimer < interactionLimit)
        {
        interactionTimer += Time.deltaTime;
        }

        
        performUpdate();
        if (poundTimer < poundTimerLimit)
        {
            poundTimer += Time.deltaTime;
        }
    }

    public void ResetInteractionTimer()
    {
        interactionTimer = 0;
    }

    public virtual void performUpdate()
    {
        
        if (!Still && interactionTimer >= interactionLimit)
        {
            UpdateAxes();
            InputHandle();
        }
        
        UpdateAnimatorParameters();
        UpdatePhysics();
        Move();
        checkInteraction();
    }

    public GameObject checkInteraction()
    {
        InteractableNPC ast = null;
        Physics.Raycast(GetComponent<Collider>().bounds.center, transform.forward * (_raycastDistance * 1.7f), out rayhit_go, _raycastDistance * 1f);
        if (rayhit_go.collider != null && rayhit_go.collider.gameObject.GetComponent<InteractableNPC>() != null)
        {
            ast = rayhit_go.collider.GetComponent<InteractableNPC>();
        }
        if (ast != null)
        {
            if (ast.Interaction != InteractableNPC.ActionStates.NOTHING)
            {
                return rayhit_go.collider.gameObject;
            }
            else
            {
                return null;
            }
        } else
        {
            return null;
        }
        
    }

    public void checkGrounded()
    {
        grounded = _controller.isGrounded;

        if (poundTimer >= poundTimerLimit && poundPhase == PoundStatus.PREPARE)
        {
            poundPhase = PoundStatus.FALLING;
        }

        if (poundPhase == PoundStatus.FALLING && (onGround() || staircased))
        {
            poundPhase = PoundStatus.NOT;
            anm.SetBool("isPounding", false);
            Camera.main.GetComponent<CharacterCam>().setShake(14, 0.2f, 0.89f, Shaker.ShakeStyle.Y, true);
        }

        if (Physics.Raycast(GetComponent<Collider>().bounds.center, Vector3.down * (_raycastDistance * 1.7f), out rayhit_s, _raycastDistance * 1.5f))
        {
            staircased = true;
        }
        else
        {
            staircased = false;
        }
    }
}
