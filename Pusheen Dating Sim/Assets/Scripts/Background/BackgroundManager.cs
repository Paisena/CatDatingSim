using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Path of the dialouge manager is always one ahead of the background manager because it runs dialogue triggger function when the app starts
//Figure out how to stop the function from running when the app starts

public class BackgroundManager : MonoBehaviour
{
    //This script controls how the background changes
    public FadeInOut fadeInOut;
    public Background background;
    public DialogueManager dialogueManager;
    public Animator animator;

    int count = 0;
    public string path = "0";

    private void Start()
    {
        path = dialogueManager.PeekNextPath();
    }

    public void checkForBackgroundChange()
    {
        //checks to see if the background needs to change after the mouse is clicked

        //if statement is to fix mouse click event triggering method 3 times for some reason idk
        if (count == 0)
        {
            //When the background needs to be changed this function will be called which goes to the FadeInOut script to change it to the next scene

            path = dialogueManager.GetPath();
            Debug.Log(path);

            if (dialogueManager.GetIfSelectingDialogue())
            {
                //path = dialogueManager.GetPreviousPath(path);
            }

            if (isBackgroundChange())
            {
                fadeInOut.FadeToNextScene(background.backgroundImage[Int32.Parse(path)]);
            }

            //increase incrementally(need to set it up so that it will follow the same path and the dialogue)
            //path++;
        }
        count++;
        if (count == 3)
        {
            count = 0;
        }

    }

    public bool isBackgroundChange()
    {
        //this method is meant to check to see if the background will change when the dialogue is clicked

        string NextDialoguePathNumber = dialogueManager.GetPath();

        if (NextDialoguePathNumber == "-1")
        {
            NextDialoguePathNumber = "0";
            return true;
        }

        // returns true if the next background image for the next dialogue is different, returns false if the background image of the current and next dialogue are the same
        if (!Int32.TryParse(NextDialoguePathNumber, out int value))
            return false;

        if (background.background.sprite != background.backgroundImage[Int32.Parse(NextDialoguePathNumber)])
        {
            return true;
        }
        return false;
    }
}