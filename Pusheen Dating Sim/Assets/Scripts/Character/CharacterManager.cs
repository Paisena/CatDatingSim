using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    //This script manages how and when the characters will appear on the screen

    //METHODS TO ADD:
    //  Reveal Character
    //  Hide Charater
    //  Change Character
    private int count = 0;
    public Sprite[] spriteList;
    public BackgroundManager backgroundManager;
    public DialogueManager dialogueManager;
    public string path = "0";
    public SpriteRenderer characterSpriteRender;

    private void Start()
    {
        path = dialogueManager.PeekNextPath();
        Debug.Log("initial path: " + path);
    }

    public void IsCharacterChange()
    {

        if (count == 0)
        {
            //This function checks to see if the character has to change and if it does it will call the ChangeCharacter function to change it
            path = dialogueManager.GetPath();

            if (dialogueManager.GetIfSelectingDialogue())
            {
                //path = dialogueManager.GetPreviousPath(path);
            }

            //Does to the check to see if the background changes to make sure it doesnt change while the screen is fading to black
            if (!backgroundManager.isBackgroundChange())
            {
                //Enabled true here because I disable it later if there is not supposed to be a character
                characterSpriteRender.enabled = true;
                if (!Int32.TryParse(path, out int value))
                    return;
                //Makes sure that the character will only change when there is something to change it to
                if (spriteList[Int32.Parse(path)] != null)
                {
                    ChangeCharacter(spriteList[Int32.Parse(path)]);
                }

                //Check to see if the spritelist dictates that there should not be a character being displayed at the moment
                if (characterSpriteRender.sprite.name == "NoBackground")
                {
                    characterSpriteRender.enabled = false;
                }
            }
            //Changes the character after waiting a bit for the screen to completely fade to black
            else
            {
                StartCoroutine(ChangeCharacterLate());
            }
        }
        count++;
        if (count == 3)
        {
            count = 0;
        }
    }

    IEnumerator ChangeCharacterLate()
    {
        //This function changes the images of the character when the screen fades to black
        //Doing it this way cause i could never figure how to use the animation
        yield return new WaitForSeconds(1.75f);
        ChangeCharacter(spriteList[Int32.Parse(path)]);

        //Wow i know im cool I used a ternary operator
        bool IsCharacterThere() => (characterSpriteRender.sprite.name == "NoBackground") ? false : true;
        characterSpriteRender.enabled = IsCharacterThere();
    }

    public void ChangeCharacter(Sprite sprite)
    {
        //the actual function which changes the sprite through the sprite renderer
        if (sprite != null)
            characterSpriteRender.sprite = sprite;
    }

    public string[] getSpriteNames()
    {
        return parseSpriteNamesToStringNames();
    }

    private string[] parseSpriteNamesToStringNames()
    {
        string[] spriteNames = new string[spriteList.Length];
        for (int i = 0; i < spriteList.Length; i++)
        {
            spriteNames[i] = spriteList[i].name;
            if(spriteList[i].name == "NoBackground"){
                spriteNames[i] = "";
            }
        }
        return spriteNames;
    }
}