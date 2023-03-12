using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //trigger for the dialogue(as the name suggests lol)
    //runs every time the mouse is clicked to start new text
    int count = 0;
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        if (count == 0 && !FindObjectOfType<BackgroundManager>().isBackgroundChange())
        {
            FindObjectOfType<DialogueManager>().startDialogue(dialogue);
        }
        count++;
        if (count == 3)
        {
            count = 0;
        }
    }
}
