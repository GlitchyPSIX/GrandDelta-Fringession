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

    private float prevSeparation;
    public float separation;
    float targetSeparation;

    public float horz_deg;
    public float ver_deg;

    public float fpsSeparation;
    public float isoSeparation;

    public enum CamMode
    {
        FPS, ISO
    }
    public float timer;
    public float rotTimer;
    public float camTimer;
    float maxCamTimer = 0f;

    public float frequency;
    public float magnitude;

    public Shaker.ShakeStyle shakestyle;

    
    public CamMode cameraMode;

    private void Start()
    {
        targetSeparation = isoSeparation;
        changeCameraType();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        rotTimer += Time.deltaTime;
        if (Input.GetKeyDown("r"))
        {
            changeCameraType();
        }
        checkInput();
        previospos = transform.position;
        previosYAngle = angle;
        previousXAngle = rotationAngle; 
    }

    public void changeCameraType()
    {
        prevSeparation = separation;
        previospos = transform.position;
        previosYAngle = angle;
        previousXAngle = rotationAngle;
        timer = 0;
        rotTimer = 0;
        if (cameraMode == CamMode.FPS)
        {
            Camera.main.nearClipPlane = -30f;
            cameraMode = CamMode.ISO;
            Camera.main.orthographic = true;
            targetYAngle = 30;
            targetXAngle = 0.75f;
            targetSeparation = isoSeparation;
        }
        else if (cameraMode == CamMode.ISO)
        {
            Camera.main.nearClipPlane = 0.01f;
            cameraMode = CamMode.FPS;
            Camera.main.orthographic = false;
            targetYAngle = 20;
            targetXAngle = 0;
            targetSeparation = fpsSeparation;
        }
    }

    private void checkInput()
    {
        if (cameraMode == CamMode.ISO)
        {
            if (Input.GetButtonDown("CamLeft"))
            {
                //Resetting the angle back
                if (rotationAngle > Mathf.PI * 2)
                {
                    rotationAngle = 0.75f;
                    targetXAngle = 0.75f + Mathf.PI / 2;
                }
                else
                {
                    targetXAngle += Mathf.PI / 2;
                }
                rotTimer = 0;

            }
            if (Input.GetButtonDown("CamRight"))
            {
                //Resetting the angle back
                if (rotationAngle - 0.75f < Mathf.PI * -2)
                {
                    rotationAngle = -0.75f;
                    targetXAngle = -0.75f - Mathf.PI / 2;
                }
                else
                {
                    targetXAngle -= Mathf.PI / 2;
                }

                rotTimer = 0;
            }
        }
        {
            if (cameraMode == CamMode.FPS)
            {
                if (Input.GetButton("CamLeft"))
                {
                    //Resetting the angle back
                    if (rotationAngle > Mathf.PI * 2)
                    {
                        rotationAngle = 0;
                        targetXAngle = 0.10f;
                    }
                    else
                    {
                        targetXAngle += 0.10f;
                    }
                }
                if (Input.GetButton("CamRight"))
                {
                    //Resetting the angle back
                    if (rotationAngle < Mathf.PI * -2)
                    {
                        rotationAngle = 0;
                        targetXAngle = -0.10f;
                    }
                    else
                    {
                        targetXAngle -= 0.10f;
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        angle = Mathf.LerpAngle(previosYAngle, targetYAngle, timer);
        rotationAngle = Mathf.Lerp(previousXAngle, targetXAngle, rotTimer);
        rotationAround = new Vector3(Mathf.Sin(rotationAngle), 0, Mathf.Cos(rotationAngle));
        separation = Mathf.Lerp(prevSeparation, targetSeparation, timer);
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
