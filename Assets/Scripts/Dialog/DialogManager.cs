using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text dialogTText;
    public Text dialogText;
    public DialogTypewriter tw;

    Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();   
    }

    public void StartDialog(Dialog dialog)
    {
        Debug.Log("Working: Starting Dialog with title: " + dialog.title);

        dialogTText.text = dialog.title;

        sentences.Clear();

        foreach(string sentence in dialog.captions)
        {
            sentences.Enqueue(sentence);
        }

        NextSentence();

    }
    
    public void NextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        string currentSentence = sentences.Dequeue();

        dialogText.text = currentSentence;
        tw.startTypewriter();
    }

    public void EndDialog()
    {
        Debug.Log("Finished dialog");
    }

}
