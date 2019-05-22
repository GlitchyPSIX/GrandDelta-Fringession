using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBlock : Beatdriven
{
    Vector3 origin;
    public Vector3 target;

    float timer;

    bool invert;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    public override void onBeat(double beat)
    {
        if (Mathf.FloorToInt((float)beat) % 4 == 1)
        {
            invert = !invert;
            timer = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < FindObjectOfType<Timeline>().beatdur)
        {
            timer += Time.deltaTime;
            Move(invert);
        }
    }

    private void Move(bool inverse)
    {
        if (!invert)
        {
            transform.position = Vector3.Lerp(origin, target, timer / (float)FindObjectOfType<Timeline>().beatdur);
        }
        else
        {
            transform.position = Vector3.Lerp(target, origin, timer / (float)FindObjectOfType<Timeline>().beatdur);
        }
        
    }
}
