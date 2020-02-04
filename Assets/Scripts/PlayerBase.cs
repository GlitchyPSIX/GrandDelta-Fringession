using System;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    internal CharacterController _controller;
    internal Animator anm;
    internal AudioSource asrc;

    public AudioClip[] audioClips;

    private bool _npc = false;

    float angl;
    public float Angle { get { return angl; } set { } }
    private Quaternion rot;

    private Vector3 gravVel;

    private Vector3 checkpoint;
    protected Vector3 spawnpoint;

    private float currSpd;
    private float spd;
    public float Speed { get { return spd; } private set { } }

    private float baseSpd;
    public float BaseSpeed { get { return baseSpd; } set { baseSpd = value; } }

    private float maxSpd;
    public float MaxSpeed { get { return maxSpd; } set { maxSpd = value; } }

    private int _hp;
    public int HP { get { return _hp; } private set { } }
    public int MaxHP { get; set; }

    protected bool grounded;
    protected bool staircased;
    protected bool rayHitsGround;
    private bool walking;

    private float interactionTimer;
    public float interactionLimit = 1f;

    private float poundTimer;
    public float poundTimerLimit = 1f;

    private float invinciTimer;
    public float invinciTimerLimit = 3f;

    public bool Invincible { get; set; }
    public bool Still { get; set; }

    public float _raycastDistance;
    private RaycastHit rayhit_s; //Raycast hit for Ground Check (Slopes, later)
    private RaycastHit rayhit_go; //Raycast hit for Object Interaction

    private float horz;
    private float vert;

    private float pureHorz;
    private float pureVert;

    float jheight;
    public float JumpHeight { get { return jheight; } set { jheight = value; } }

    public bool NPC { get => _npc; set => _npc = value; }

    public enum AirStatus
    {
        GROUND, GP_PREPARE, GP_FALLING, MIDAIR
    }

    public enum TauntStatus
    {
        NONE, DANCE, PRAISE
    }

    public enum HurtStatus
    {
        ALIVE, HURT, DEAD
    }

    TauntStatus tauntStatus = TauntStatus.NONE;
    HurtStatus hurtStatus = HurtStatus.ALIVE;
    AirStatus poundPhase = AirStatus.GROUND;

    internal Vector3 move;
    internal Vector3 unfilteredMove;

    [Obsolete("This will be removed later.")]
    public bool onGround()
    {

        return grounded;

        /* We determine if the character is on ground by checking if
         1, the capsule cast below collides with *something*,
         2, the angle of the slope is within the acceptable margin
         In case we were not on a slope and we're, indeed, floating somewhere
         we slide down said slope later in the code.
        */
    }

    void OnCollisionStay(Collision hit)
    {
        //Update checkpoint if we hit a checkpoint
        if (hit.gameObject.tag == "Checkpoint")
        {
            checkpoint = transform.position;
        }

        //If we collide with an object that hurts us
        if (hit.gameObject.tag == "Hurt" && hurtStatus == HurtStatus.ALIVE && invinciTimer >= invinciTimerLimit)
        {
            Hurt();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.min.y, GetComponent<Collider>().bounds.center.z), 0.45f);
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

        if (poundPhase == AirStatus.GROUND && hurtStatus != HurtStatus.DEAD)
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
        else if (poundPhase == AirStatus.MIDAIR && hurtStatus != HurtStatus.DEAD)
        {
            move = Vector3.ClampMagnitude(move + ((movex+movez)*0.05f), baseSpd/8);
            move = new Vector3(move.x, 0, move.z);

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
        anm.SetBool("isGrounded", grounded || staircased);
        anm.SetFloat("vertMomentum", gravVel.y);
        anm.SetBool("isWalking", walking);
        anm.SetInteger("hurtStatus", (int)hurtStatus);
        anm.SetInteger("tauntStatus", (int)tauntStatus);
        if (walking)
        {
            tauntStatus = TauntStatus.NONE;
        }
    }

    public void UpdatePhysics()
    {
        //Gravity accel
        gravVel.y += Physics.gravity.y * 2 * Time.deltaTime;

        //Restore gravity accel if grounded
        if (((grounded || staircased) && !rayHitsGround)
            && gravVel.y < 0)
        {
            gravVel.y = 0;
        }

        //Gravity goes before any other movement
        if (hurtStatus != HurtStatus.DEAD)
        {
            if (poundPhase != AirStatus.GP_PREPARE)
            {
                _controller.Move(gravVel * Time.deltaTime);
            }
            else
            {
                gravVel.y += Physics.gravity.y * 8 * Time.deltaTime;
            }
        }
    }

    public void Jump(float multiplier = 1f)
    {
        poundPhase = AirStatus.MIDAIR;
        gravVel.y = (jheight * multiplier * Physics.gravity.y * -2);
    }

    public void InputHandle()
    {
        if (!_npc)
        {
            //If dead, and pressed A (Only available until a proper menu is made)
            if (Input.GetButtonDown("Interact") && hurtStatus == HurtStatus.DEAD)
            {
                Necromancy(false);
                Respawn(false, false);
            }

            //Jumping
            if (Input.GetButtonDown("Jump") && (grounded || staircased))
            {
                Jump();
            }

            //Running
            if (Input.GetButton("Run") && (grounded || staircased))
            {
                spd = Mathf.SmoothDamp(spd, maxSpd, ref currSpd, 0.1f);
            }
            else if (!Input.GetButton("Run") && !(grounded || staircased))
            {
                spd = Mathf.SmoothDamp(spd, baseSpd, ref currSpd, 2f);
            }
            else if (!Input.GetButton("Run"))
            {
                spd = Mathf.SmoothDamp(spd, baseSpd, ref currSpd, 0.1f);
            }


            //Groundpound
            if (Input.GetButtonDown("Pound") && (!(grounded || staircased) || poundPhase == AirStatus.MIDAIR))
            {
                anm.SetBool("isPounding", true);
                move = Vector3.zero;
                gravVel = Vector3.zero;
                anm.Play("Groundpound");
                poundPhase = AirStatus.GP_PREPARE;
                poundTimer = 0;
            }

            //Interact with NPCs/items
            if (Input.GetButtonDown("Interact") && (grounded || staircased))
            {
                GameObject interactable = checkInteraction();
                if (interactable != null)
                {
                    interactable.GetComponent<InteractableNPC>().Action();
                }
            }

            //Taunting
            if (Input.GetButtonDown("Taunt") && (grounded || staircased))
            {
                anm.Play("Taunt");
            }

            //If we're touching any axis and we're in the ground
            if (unfilteredMove.magnitude != 0 && poundPhase == AirStatus.GROUND)
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

    public void Necromancy(bool kill = false)
    {
        if (!kill)
        {
            Heal(MaxHP);
            interactionTimer = 0;
            hurtStatus = HurtStatus.ALIVE;
            tauntStatus = TauntStatus.NONE;
        }
        else
        {
            if (hurtStatus == HurtStatus.DEAD)
            {
                Die();
            }
            else
            {
                Debug.Log("Cannot kill the dead!");
            }
            
        }
    }

    public virtual void Move()
    {
        /* 
         * 
         * TODO: Slope slide logic
        */

        //Now we move
        if (poundPhase != AirStatus.GP_PREPARE && hurtStatus != HurtStatus.DEAD)
        {
            _controller.Move(move * Time.deltaTime * spd);
            Debug.Log(move);
        }
        checkGrounded();
    }

    private void Update()
    {
        if (interactionTimer < interactionLimit)
        {
            interactionTimer += Time.deltaTime;
        }

        if (interactionTimer >= interactionLimit && hurtStatus == HurtStatus.HURT)
        {
            hurtStatus = HurtStatus.ALIVE;
        }
        else if (interactionTimer < interactionLimit && hurtStatus == HurtStatus.HURT)
        {
            hurtStatus = HurtStatus.HURT;
        }

        if (invinciTimer < invinciTimerLimit && hurtStatus == HurtStatus.ALIVE)
        {
            invinciTimer += Time.deltaTime;
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
        
        UpdatePhysics();
        Move();
        UpdateAnimatorParameters();
        checkInteraction();
    }

    public void Heal(int amount)
    {
        asrc.PlayOneShot(audioClips[2], 0.7f);
        _hp += amount;
        if (_hp > MaxHP)
        {
            _hp = MaxHP;
        }
        FindObjectOfType<Healthbar>().GetComponent<Healthbar>().changeHP(false);
    }

    public void Hurt()
    {
        if (_hp > 1)
        {
            asrc.PlayOneShot(audioClips[0], 0.7f);
            Camera.main.GetComponent<CharacterCam>().setShake(20, 0.4f, 0.8f, Shaker.ShakeStyle.Y);
            anm.Play("Hurt");
            move = -move;
            Debug.Log("Hurt!");
            Jump(0.5f);
            hurtStatus = HurtStatus.HURT;
            invinciTimer = 0;
            interactionTimer = 0;
            _hp--;
        }
        else
        {
            _hp--;
            Die();
        }
        FindObjectOfType<Healthbar>().GetComponent<Healthbar>().changeHP(true);
        
    }

    public void Die()
    {
        asrc.PlayOneShot(audioClips[1], 0.7f);
        hurtStatus = HurtStatus.DEAD;
        Camera.main.GetComponent<CharacterCam>().setShake(20, 0.6f, 0.8f, Shaker.ShakeStyle.RADIUS);
        anm.Play("Dead");
        move = Vector3.zero;
        gravVel = Vector3.zero;
        Debug.Log("Dead!");
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

    public void Respawn(bool atCheckpoint = true, bool hurt = true)
    {
        move = Vector3.zero;
        gravVel = Vector3.zero;
        if (hurt)
        {
            Hurt();
        }
        _controller.enabled = false; //Disable Controller for a bit to allow warping
        if (atCheckpoint)
        {
           
            transform.position = checkpoint;
        }
        else
        {
            transform.position = spawnpoint;
        }
        _controller.enabled = true; //Re-enable Controller
    }

    public virtual void checkGrounded()
    {

        if (gravVel.y <= 0)
        {
            grounded = Physics.CheckSphere(new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.min.y, GetComponent<Collider>().bounds.center.z), 0.10f, ~(1 << 10), QueryTriggerInteraction.Ignore);

            if (Physics.CheckSphere(new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.min.y, GetComponent<Collider>().bounds.center.z), 0.45f, ~(1 << 10), QueryTriggerInteraction.Ignore))
            {
                staircased = true;
                if (poundPhase != AirStatus.GP_FALLING)
                {
                    poundPhase = AirStatus.GROUND;
                }
            }
            else
            {
                staircased = false;
            }
            rayHitsGround = (Physics.Raycast(GetComponent<Collider>().bounds.center,
                Vector3.down * (_raycastDistance * 1.7f), out rayhit_s, _raycastDistance * 1.5f));
        }
        else
        {
            grounded = false;
            staircased = false;
        }

        if (poundTimer >= poundTimerLimit && poundPhase == AirStatus.GP_PREPARE)
        {
            poundPhase = AirStatus.GP_FALLING;
        }

        if (poundPhase == AirStatus.GP_FALLING && (grounded || staircased || rayHitsGround))
        {
            asrc.PlayOneShot(audioClips[3], 0.5f);
            poundPhase = AirStatus.GROUND;
            anm.SetBool("isPounding", false);
            Camera.main.GetComponent<CharacterCam>().setShake(14, 0.2f, 0.89f, Shaker.ShakeStyle.Y, true);
        }

    }
}
