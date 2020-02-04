using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDeltaController : MonoBehaviour, IGravitable, IHealth, IMovable
{

    private CharacterController _controller;

    // Variables for movement
    private float horz;
    private float vert;
    private float pureHorz;
    private float pureVert;

    // Gravity
    private Vector3 gravityVector;

    //Movement
    [SerializeField]
    private Vector3 movementVector;

    [SerializeField]
    private float _speed;

    // ----------------- END OF VARIABLE DECLARATION -----------------

    void UpdateInputAxes()
    {
        horz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        pureHorz = Input.GetAxisRaw("Horizontal");
        pureVert = Input.GetAxisRaw("Vertical");

        Vector3 movez = horz * Camera.main.transform.right;
        Vector3 movex = vert * Camera.main.transform.forward;

        Vector3 moveZ_pure = pureHorz * Camera.main.transform.right;
        Vector3 moveX_pure = pureVert * Camera.main.transform.forward;

        movementVector = new Vector3((movez + movex).x, 0, (movez + movex).z);
        Debug.Log(movementVector);
    }
    
    //Discrete inputs are button presses
    void UpdateInputDiscrete()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump(0.5f);
        }
    }

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        UpdatePhysics();
        Move((movementVector + gravityVector) * Time.fixedDeltaTime * _speed);
    }

    void Update()
    {
        UpdateInputDiscrete();
        UpdateInputAxes();
    }

    public bool CheckGround()
    {
        throw new System.NotImplementedException();
    }

    public void Jump(float amount)
    {
        gravityVector.y = (amount * Physics.gravity.y * -2);
    }

    public void Move(Vector3 direction)
    {
        _controller.Move(direction);
    }

    public void UpdatePhysics()
    {
        //Gravity accel
        gravityVector.y += Physics.gravity.y * 2 * Time.fixedDeltaTime;


        //Gravity goes before any other movement
        /*if (hurtStatus != HurtStatus.DEAD)
        {
            if (poundPhase != AirStatus.GP_PREPARE)
            {
                _controller.Move(gravVel * Time.deltaTime);
            }
            else
            {
                gravVel.y += Physics.gravity.y * 8 * Time.deltaTime;
            }
        }*/
    }

    public void Damage()
    {
        throw new System.NotImplementedException();
    }

    public void Heal()
    {
        throw new System.NotImplementedException();
    }

    public void Kill()
    {
        throw new System.NotImplementedException();
    }

    public void Resucitate()
    {
        throw new System.NotImplementedException();
    }



}
