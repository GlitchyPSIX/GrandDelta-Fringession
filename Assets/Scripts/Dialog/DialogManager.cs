using System;
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
    /// <summary>
    /// Starts a dialog normally.
    /// </summary>
    /// <param name="dialog">The dialog to start</param>
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

    /// <summary>
    /// Starts the dialog, but performing an Action first.
    /// </summary>
    /// <param name="dialog">The dialog to start</param>
    /// <param name="act">The Action to perform</param>
    public void StartDialog(Dialog dialog, Action act)
    {
        act();
        StartDialog(dialog);
    }

    public void NextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        string currentSentence = sentences.Dequeue();

        if (currentSentence.StartsWith("#"))
        {
            tw.blankTypewriter();
            string[] splitAction = currentSentence.Split('!');
            string objectName = splitAction[0];
            string message = splitAction[1];
            GameObject go = GameObject.Find(objectName);
            go.SendMessage(message);
        }
        else if (currentSentence.StartsWith("="))
        {
            string[] splitSubject = currentSentence.Split('>');
            string subject = splitSubject[0];
            string followingText = splitSubject[1];
            dialogTText.text = subject.TrimStart('=');
            if (followingText != "")
            {
                dialogText.text = followingText;
                tw.startTypewriter();
            }
            else
            {
                currentSentence = sentences.Dequeue();
                dialogText.text = currentSentence;
                tw.startTypewriter();
            }

        }
        else
        {
        dialogText.text = currentSentence;
        tw.startTypewriter();
        }
        
    }

    public void EndDialog()
    {
        Debug.Log("Finished dialog");
    }

}
