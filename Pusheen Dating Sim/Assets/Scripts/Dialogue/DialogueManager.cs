using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
public class DialogueManager : MonoBehaviour
{

    [SerializeField]
    private string gotToPath = "0";
    string[] sentences;
    string[] pathOrder;
    string[] dialogueOptions;
    string[] characterNames;
    public string NextDialoguePathNumber = "-1";

    int count = 0;

    public bool selectingDialogue;

    [SerializeField]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI characterNameText;

    public Animator animator;

    public BackgroundManager backgroundManager;

    public CharacterManager characterManager;

    string characterName;

    private bool backgroundChanges = false;
    private void Awake() {
        SetPath();
    }
    private void Start()
    {
        selectingDialogue = false;
    }

    public void startDialogue(Dialogue dialogue)
    {
        //pushes all data from the dialogue table to the respective versions in this class

        //set up the array which will hold all the dialogue and where each text message will go
        sentences = new string[dialogue.sentence.Length];
        pathOrder = new string[dialogue.path.Length];
        dialogueOptions = new string[dialogue.dialogueOptions.Length];
        characterNames = new string[dialogue.name.Length];

        //insert the dialogue sentences and the paths into the array
        for (int i = 0; i < characterNames.Length; i++)
        {
            characterNames[i] = dialogue.name[i];
        }
        for (int i = 0; i < sentences.Length; i++)
        {
            sentences[i] = dialogue.sentence[i];
        }
        for (int i = 0; i < pathOrder.Length; i++)
        {
            pathOrder[i] = dialogue.path[i];
        }
        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            dialogueOptions[i] = dialogue.dialogueOptions[i];
        }

        //start displaying the dialogue
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        //this function determines which dialogue will be displayed

        //variable which decides which dialogue in the array of dialogue will be displayed
        string dialogueNumber = "-2";

        //call the function which returns what dialogue that needs to be displayed
        if (!selectingDialogue)
        {
            dialogueNumber = GetNextPath();

            if (backgroundManager.isBackgroundChange() && !backgroundChanges)
            {
                Debug.Log("background change");
                backgroundChanges = true;
                return;
            }

            if (backgroundChanges)
            {
                dialogueNumber = GetPreviousPath(dialogueNumber);
                NextDialoguePathNumber = GetPreviousPath(NextDialoguePathNumber);
                backgroundChanges = false;

            }

            //check to see if there is multiple paths the dialogue can go down and if so then activate the option dialogue
            if (dialogueNumber.Contains(","))
            {
                string[] splitPath = GetDialogueOptions(dialogueNumber);
                int[] splitPathInt = new int[splitPath.Length];

                for (int i = 0; i < splitPath.Length; i++)
                {
                    splitPathInt[i] = Int32.Parse(splitPath[i]);
                }

                string[] optionString = new string[splitPath.Length];

                for (int i = 0; i < optionString.Length; i++)
                {
                    Debug.Log(dialogueOptions[splitPathInt[i]]);
                    optionString[i] = dialogueOptions[splitPathInt[i]];
                }
                displayOptions(optionString);
                return;
            }
        }

        //take the dialogue which will be displayed out from the array of dialogues
        string sentence;
        if (!selectingDialogue)
        {
            sentence = sentences[Int32.Parse(dialogueNumber)];
            characterName = characterNames[Int32.Parse(dialogueNumber)];
        }
        else
        {
            sentence = sentences[Int32.Parse(NextDialoguePathNumber)];
            characterName = characterNames[Int32.Parse(NextDialoguePathNumber)];
            Debug.Log(characterName);
            if (characterName == "DialogueManager")
            {
                characterName = "";
            }
        }
        //display the dialogue
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, characterName));
    }

    private string GetNextPath()
    {
        //this function finds what the path number of the next number to be displayed will be

        //for when the who dialogue system begins it will return the first dialogue
        if (Int32.Parse(NextDialoguePathNumber) is -1)
        {
            NextDialoguePathNumber = "0";
            return NextDialoguePathNumber;
        }

        //finds the path to be returned from the path array
        string nextPath = pathOrder[Int32.Parse(NextDialoguePathNumber)];

        //prepares the next path to be found when it is called again
        NextDialoguePathNumber = nextPath;

        return nextPath;
    }

    IEnumerator TypeSentence(string sentence, string characterName)
    {
        // Places individual letters from dialogue into text box
        characterNameText.text = characterName;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator TypeSentences(string[] sentences, string characterName)
    {
        // Places individual letters from dialogue into text box
        characterNameText.text = characterName;
        dialogueText.text = "";
        foreach (string sentence in sentences)
        {
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(0.01f);
            }
            dialogueText.text += "\n";
        }
    }

    public void EndDialogue()
    {

    }

    public string PeekNextPath()
    {
        if (!Int32.TryParse(NextDialoguePathNumber, out int value))
            return null;
        if (Int32.Parse(NextDialoguePathNumber) is -1)
            return "0";
        else
        {
            return pathOrder[Int32.Parse(NextDialoguePathNumber)];
        }
    }

    public string GetPath()
    {
        return NextDialoguePathNumber;
    }

    public string[] GetDialogueOptions(string path)
    {
        //return string which will give back them sepereated
        //this function will seperate the given variable and give it back for the script to display the dialogue options to the user
        string removedPathString = path.Replace(",", "");

        string[] pathSplit = path.Split(",");

        return pathSplit;
    }

    public void displayOptions(string[] options)
    {
        count = 0;
        selectingDialogue = true;
        StopAllCoroutines();
        StartCoroutine(TypeSentences(options, characterName));
        //TypeSentences(options);
    }

    public void selectDialogue(string option)
    {

        if (count != 0)
        {
            return;
        }

        if (!selectingDialogue)
            return;


        string[] dialogueOptions = GetDialogueOptions(NextDialoguePathNumber);


        NextDialoguePathNumber = GetPreviousPath(pathOrder[Int32.Parse(dialogueOptions[Int32.Parse(option)])]);

        if (!backgroundManager.isBackgroundChange())
        {
            DisplayNextSentence();
        }
        else
        {
            Debug.Log("changed during screen fade");
        }
        characterManager.IsCharacterChange();
        backgroundManager.checkForBackgroundChange();


        selectingDialogue = false;
        count++;

    }

    public bool GetIfSelectingDialogue()
    {
        return selectingDialogue;
    }

    public string GetPreviousPath(string path)
    {
        for (int i = 0; i < pathOrder.Length; i++)
        {
            if (pathOrder[i] == path)
            {
                return i.ToString();
            }
        }

        Debug.Log("no previous path");
        return "no previous path";
    }

    private void SetPath()
    {
        NextDialoguePathNumber = gotToPath;
    }
}