using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredTrigger : MonoBehaviour
{

    public float volume;
    public int layer;

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<LayeredAudioSource>().SetSampleVolume(layer, volume);
        Destroy(gameObject);
    }

}
