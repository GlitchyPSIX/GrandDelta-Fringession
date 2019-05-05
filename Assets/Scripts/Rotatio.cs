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
        tr.Rotate(Vector3.forward, 10 * Time.deltaTime);
        tr.Translate(Vector3.up * Time.deltaTime);
    }
}
