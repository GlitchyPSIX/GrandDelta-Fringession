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
    public Animator anm;

    Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        tw.Status = DialogTypewriter.TypewriterStatus.INACTIVE;
        sentences = new Queue<string>();   
    }
    /// <summary>
    /// Starts a dialog normally.
    /// </summary>
    /// <param name="dialog">The dialog to start</param>
    public void StartDialog(Dialog dialog)
    {
        tw.Status = DialogTypewriter.TypewriterStatus.IDLE;
        Debug.Log("Working: Starting Dialog with title: " + dialog.title);
        anm.SetBool("Done", false);
        anm.Play("Enter");
        anm.SetBool("Displaying", false);

            Start();

        dialogTText.text = dialog.title;

        sentences.Clear();

        foreach(string sentence in dialog.captions)
        {
            sentences.Enqueue(sentence);
        }
        tw.blankTypewriter();
        anm.SetBool("Done", false);
        anm.SetBool("Displaying", true);
        NextSentence();
    }

    private void Update()
    {
     if (Input.GetButtonDown("Interact") && tw.Status == DialogTypewriter.TypewriterStatus.IDLE)
        {
            NextSentence();
        }
            if (Input.GetButtonDown("Run"))
            {
                tw.speed = tw.maxspeed / 8;
            }
            else
            {
                tw.speed = tw.maxspeed;
            }
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
        anm.SetBool("Displaying", false);
        tw.Status = DialogTypewriter.TypewriterStatus.INACTIVE;
        Debug.Log("Finished dialog");
    }

}
