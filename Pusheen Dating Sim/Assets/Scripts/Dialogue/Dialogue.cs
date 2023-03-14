using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Dialogue
{

    //this
    public string[] name;

    [TextArea(3, 10)]
    public string[] sentence;
    public string[] path;

    public string[] dialogueOptions;
}