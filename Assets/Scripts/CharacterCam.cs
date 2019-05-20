using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCam : MonoBehaviour
{

    public GameObject followingObject;
    public float distanceZ;
    public float distanceY;
    public float angle;

    Vector3 rotationAround;
    Vector3 previospos;
    Vector3 originalpos;
    Vector3 originalPosition;

    public float previousXAngle;
    public float previosYAngle;
    public float targetXAngle;
    public float targetYAngle;
    public float rotationAngle;
    public float separation;
    public float horz_deg;
    public float ver_deg;
    public enum CamMode
    {
        FPS, ISO
    }
    public float timer;
    public float camTimer;
    float maxCamTimer = 0f;

    public float frequency;
    public float magnitude;

    public Shaker.ShakeStyle shakestyle;

    
    public CamMode cameraMode;

    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown("r"))
        {
            changeCameraType();
        }
        previospos = transform.position;
        previosYAngle = angle;
        previousXAngle = rotationAngle; 
    }

    public void changeCameraType()
    {
        previospos = transform.position;
        previosYAngle = angle;
        previousXAngle = rotationAngle;
        timer = 0;
        if (cameraMode == CamMode.FPS)
        {
            Camera.main.orthographic = true;
            targetYAngle = 30;
            targetXAngle = 0.75f;
        }
    }

    private void LateUpdate()
    {
        angle = Mathf.LerpAngle(previosYAngle, targetYAngle, timer);
        rotationAngle = Mathf.LerpAngle(previousXAngle, targetXAngle, timer);
        rotationAround = new Vector3(Mathf.Sin(rotationAngle), 0, Mathf.Cos(rotationAngle));
        horz_deg = -180 + Mathf.Rad2Deg * Mathf.Atan2((rotationAround.x * separation), (rotationAround.z * separation));
        originalpos = Vector3.Lerp(previospos, new Vector3(followingObject.transform.position.x + (rotationAround.x * separation), followingObject.transform.position.y + Mathf.Abs(separation)/2, followingObject.transform.position.z + (rotationAround.z * separation)), timer);
        transform.eulerAngles = new Vector3(angle, horz_deg, transform.eulerAngles.z);

        // Shaker but for Character Camera
        if (camTimer < maxCamTimer)
        {
            camTimer += Time.deltaTime;

            float value = Mathf.Cos((camTimer / maxCamTimer) * Mathf.PI * frequency);
            value = value * Mathf.Cos((camTimer / maxCamTimer) * (Mathf.PI / 2)) * magnitude;
            Vector3 pos = Vector3.zero;
            if (shakestyle == Shaker.ShakeStyle.X)
            {
                pos = transform.right * value;
            }
            else if (shakestyle == Shaker.ShakeStyle.Y)
            {
                pos = transform.up * value;
            }
            else if (shakestyle == Shaker.ShakeStyle.RADIUS)
            {
                pos = Random.insideUnitCircle * value;
            }
            originalpos = originalpos + pos;
        }
        transform.position = originalpos;

        if (cameraMode != CamMode.ISO)
        {
            if (Input.GetKey("q"))
            {
                targetXAngle += 0.05f;
            }
            if (Input.GetKey("e"))
            {
                targetXAngle -= 0.05f;
            }
        }
        else
        {

        }
    }

    public void setShake(float freq, float time, float amnt, Shaker.ShakeStyle shaketype, bool restart = true)
    {
        frequency = freq;
        maxCamTimer = time;
        magnitude = amnt;
        shakestyle = shaketype;
        if (restart)
        {
            camTimer = 0;
        }

    }

}
