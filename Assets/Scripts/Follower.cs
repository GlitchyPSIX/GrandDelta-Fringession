using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{

    public GameObject followingObject;
    public float distanceZ;
    public float distanceY;
    public float angle;
    Vector3 rotationAround;
    public float rotationAngle;
    public float separation;
    public float horz_deg;
    public float ver_deg;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void LateUpdate()
    {
        rotationAround = new Vector3(Mathf.Sin(rotationAngle), 0, Mathf.Cos(rotationAngle));
        horz_deg = -180 + Mathf.Rad2Deg * Mathf.Atan2((rotationAround.x * separation), (rotationAround.z * separation));
        transform.position = new Vector3(followingObject.transform.position.x + (rotationAround.x * separation), followingObject.transform.position.y + distanceY, followingObject.transform.position.z + (rotationAround.z * separation));
        transform.eulerAngles = new Vector3(angle, horz_deg, transform.eulerAngles.z);
        
        if (Input.GetKey("q"))
        {
            rotationAngle += 0.05f;
        }
        if (Input.GetKey("e"))
        {
            rotationAngle -= 0.05f;
        }
    }
}
