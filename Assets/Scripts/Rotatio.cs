using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatio : MonoBehaviour
{

    Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = transform;    
    }

    // Update is called once per frame
    void Update()
    {
        tr.Rotate(Vector3.forward, 30 * Time.deltaTime);
    }
}
