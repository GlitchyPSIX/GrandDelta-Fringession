using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Conductor : MonoBehaviour
{

    //This class holds the beat duration.
    //thanks, fizzd, I owe you a big one.

    public float bpm = 130;
    public float offset = 0;
    public double songposition;
    double dspOffset = 0d;
    public float pitch;
    public AudioListener gameSpeaker;
    Timeline timeline;

    // Use this for initialization
    void Start()
    {
        timeline = GetComponent<Timeline>();
        resetStartOfSong(true);
    }

    // Update is called once per frame
    void Update()
    {
        songposition = ((AudioSettings.dspTime) - dspOffset) * pitch - offset;
    }

    public void resetStartOfSong(bool pauseAtStart){
        dspOffset = AudioSettings.dspTime;
        if (pauseAtStart) { 
            AudioListener.pause = true;
        }
    }

    public IEnumerator resetBeatmap()
    {
        timeline.beatcount = 1;
        yield return null;
    }

}
