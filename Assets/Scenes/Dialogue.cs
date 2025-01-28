using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    // Speaker's name
    public string[] name;

    // Code for event that should happen on this dialogue
    // E.g, change sprite, change screen

    [TextArea(3,10)]
    public string[] sentences;

    public int[] code;
}
