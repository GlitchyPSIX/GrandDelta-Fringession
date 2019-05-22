using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredAudioSource : MonoBehaviour

{

    public GameObject[] Samples;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject clip in Samples)
        {
            clip.GetComponent<AudioSource>().volume = 0;
            clip.GetComponent<AudioSource>().Play();
        }
    }

    public void SetSampleVolume(int sample, float volume)
    {
        Samples[sample].GetComponent<AudioSource>().volume = volume;
    }

    public void ChangePitch(float pitch)
    {
        foreach (GameObject clip in Samples)
        {
            clip.GetComponent<AudioSource>().pitch = pitch;
        }
    }
}
