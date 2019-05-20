using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    float maxTime;
    float timer;
    float frequency;
    float magnitude;

    public enum ShakeStyle
    {
        X, Y, RADIUS
    }

    ShakeStyle shakestyle;

    Vector3 originalPosition;

    // Start is called before the first frame update
    void OnEnable()
    {
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < maxTime)
        {
            timer += Time.deltaTime;

            float value = Mathf.Cos((timer / maxTime) * Mathf.PI * frequency);
            value = value * Mathf.Cos((timer / maxTime) * (Mathf.PI / 2)) * magnitude;
            Vector3 pos = Vector3.zero;
            if (shakestyle == ShakeStyle.X)
            {
                pos = new Vector3(value, 0);
            }
            else if (shakestyle == ShakeStyle.Y)
            {
                pos = new Vector3(0, value);
            }
            else if (shakestyle == ShakeStyle.RADIUS)
            {
                pos = Random.insideUnitCircle * value;
            }
            transform.localPosition = originalPosition + pos;
        }
    }

    public void setShake(float freq, float time, float amnt, ShakeStyle shaketype, bool restart = true)
    {
        frequency = freq;
        maxTime = time;
        magnitude = amnt;
        shakestyle = shaketype;
        if (restart)
        {
            timer = 0;
        }
        
    }
}
