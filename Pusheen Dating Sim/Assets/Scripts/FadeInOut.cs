using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FadeInOut : MonoBehaviour
{
    //this script will control how the game fades in and out when switches backgrounds

    public Animator animator;
    public Background background;
    public Sprite backgroundSprite;
    public DialogueTrigger dialogueTrigger;
    public TextMeshProUGUI text;
    public DialogueManager dialogueManager;
    public BackgroundManager backgroundManager;
    public CharacterManager characterManager;

    public void FadeToNextScene(Sprite backgroundImg)
    {
        //this function tells the animator to turn the screen to black
        //pairs with the function OnFadeComplete which will turn the screen out from black back to normal 
        animator.SetBool("Fade", true);
        backgroundSprite = backgroundImg;
    }

    public void OnFadeComplete()
    {
        //this function tells the animator to turn screen from black to normal
        //runs after the function FadeToNextScene plays
        animator.SetBool("Fade", false);

        //also resets the dialogue text box and changes the background image since this function runs at the start of when the screen returns to normally meaning that it will be 
        //completely black when the function is run  
        background.background.sprite = backgroundSprite;
        text.text = "";
        //characterManager.IsCharacterChange();
    }

    public void inNextScene()
    {
        //this function will tell the dialogue manager script to run the dialogue after the screen finishes fading back in
        //dialogueManager.firstFadeDone = false;
        FindObjectOfType<DialogueManager>().startDialogue(dialogueTrigger.dialogue);
        //backgroundManager.path = dialogueManager.GetPath();
        //characterManager.path = dialogueManager.GetPath();
        //characterManager.IsCharacterChange();
    }

    public void IsChangeCharacterFade(){
        Debug.Log("fadeinoutmethod called");
        characterManager.IsCharacterChange();
        //characterManager.ChangeCharacter(characterManager.spriteList[characterManager.path]);
    }
}
