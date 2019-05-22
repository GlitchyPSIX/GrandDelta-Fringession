using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatUI : Beatdriven
{

    public Sprite[] numbers = { null, null, null, null };

    private Image _beatImg;

    public override void onBeat(double beat)
    {
        _beatImg.sprite = numbers[Mathf.FloorToInt((float)beat) % 4];
    }

    // Start is called before the first frame update
    void Start()
    {
        _beatImg = GetComponent<Image>();   
    }
}
