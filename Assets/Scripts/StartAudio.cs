using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAudio : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<Timeline>().StartSong();
    }
}
