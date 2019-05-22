using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAudio : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (AudioListener.pause)
        {
            FindObjectOfType<Timeline>().StartSong();
            FindObjectOfType<LayeredAudioSource>().SetSampleVolume(0, 1);
            FindObjectOfType<LayeredAudioSource>().SetSampleVolume(1, 1);
            FindObjectOfType<LayeredAudioSource>().SetSampleVolume(2, 1);
            FindObjectOfType<LayeredAudioSource>().SetSampleVolume(3, 1);
        }
        
    }
}
