using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadDemo : MonoBehaviour
{
    AudioSource asc;
    bool Loading = false;
    AsyncOperation loadingScene;
    Text txt;
    public AudioClip loadingSound;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
        asc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && Loading == false)
        {
            Loading = true;
            asc.PlayOneShot(loadingSound);
            loadingScene = SceneManager.LoadSceneAsync("SampleScene");
        }
        if (Loading)
        {
            txt.text = "Loading scene: " + loadingScene.progress * 100 + "%.";
        }
    }
}
