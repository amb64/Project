using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    // Speaker's name
    public string[] name;

    // If this event should add / subtract from a stat at the end of it, fill this out in the dialogue.
    // In order of stats on the screen, 1 energy, 2 stress, 3 anxiety, 4 depression
    public int statToAffect;
    public int effectAmount;

    // If true, this event is part of a sequence of events in the same timeslot, and therefore the "new timeslot" popup shouldnt appear
    public bool doesntNeedPopup;

    // The dialogue text
    [TextArea(3,10)]
    public string[] sentences;

    // Code for event that should happen on this dialogue
    // E.g, change sprite, change screen
    public int[] code;

}
