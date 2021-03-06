﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    //This class should hold all the actions that happen at certain times, and perform them in time.

    public double pastbeat;
    public double offset = 0;
    public double beatcount = 0f;
    public double beatmultiplier;
    public double beatdur;
    Conductor conductor;
    double pastRevolution;
    public float snap;

    public List<Beatdriven> beatObjects;

    public double nextbeat;

    void Start()
    {
        beatmultiplier = 1f;
        beatdur = (60 / GetComponent<Conductor>().bpm);
        conductor = GetComponent<Conductor>();
        snap = 1f;
        nextbeat += beatdur * beatmultiplier;
        beatObjects.AddRange(FindObjectsOfType<Beatdriven>());
    }

    void Update()
    {
        updateTimelinePosition();
        // every beat (With the multiplier in action, probably gonna use this for swing beats)

        if (GetComponent<Conductor>().songposition + offset > pastbeat + (beatdur * beatmultiplier))
        {
            foreach (Beatdriven bdriven in beatObjects)
            {
                bdriven.onBeat(beatcount);
            }
            pastbeat += beatdur * beatmultiplier;
            nextbeat += beatdur * beatmultiplier;
        }
    }

    public void StartSong()
    {
        //start song
        conductor.pitch = 1;
        conductor.resetStartOfSong(false);
        conductor.resetBeatmap();
            AudioListener.pause = false;
    }

    public void updateNextBeat(string reason = "")
    {
        nextbeat += beatdur * beatmultiplier;
        //Debug: to check why did the next beat get updated
        Debug.Log("Updated nextbeat to: " + nextbeat + " (" + reason + ")");
    }

    public void updateTimelinePosition()
    {
        if (GetComponent<Conductor>().songposition + offset > pastRevolution + (beatdur * snap))
        {
            beatcount += snap;
            pastRevolution += (beatdur * snap);
        }
    }

}
