using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int day = 1;
    private int timeslot = 0;

    public List<GameObject> events;
    private GameObject current_event;
    private DialogueTrigger trigger;

    private DialogueManager manager;


    public GameObject roomScreen;
    public GameObject computerScreen;
    public GameObject chatScreen;
    
    public GameObject current_screen;

    // Start is called before the first frame update
    void Start()
    {
        // Get the dialogue manager
        manager = FindObjectOfType<DialogueManager>();

        // Current screen, same as in the code atlas. Starts at 1 signifiying the room screen.
        current_screen = roomScreen;

        // Start the first event
        NextEvent();
    }

    // Update is called once per frame
    void Update()
    {
        // If the current dialogue is finished, start the next one
        if(manager.fin == true)
        {
            timeslot += 1;
            Debug.Log("Next timeslot. Current index :" + timeslot);

            // Free time timeslot needs to go here

            NextEvent();
        }

        //Debug.Log("Dialogue ended? - " + manager.fin);

        switch(manager.code)
        {
            case 0:
                break;
            case 1:
                NextEvent();
                break;
            case 2:
                // Swap to room screen
                current_screen.SetActive(false);
                roomScreen.SetActive(true);
                current_screen = roomScreen;
                //NextEvent();
                break;
            case 3:
                // Swap to computer screen
                current_screen.SetActive(false);
                computerScreen.SetActive(true);
                current_screen = computerScreen;
                //NextEvent();
                break;
            case 4:
                // Swap to chat screen
                current_screen.SetActive(false);
                chatScreen.SetActive(true);
                current_screen = chatScreen;
                //NextEvent();
                break;
        }
    }

    void NextEvent()
    {
        // Get the next event trigger and trigger it
        try
        {
            current_event = events[timeslot];
            trigger = current_event.GetComponent<DialogueTrigger>();
            Debug.Log("Trigger object is: " + trigger.gameObject.name);

            trigger.TriggerDialogue();
        }
        catch(ArgumentOutOfRangeException)
        {
            Debug.Log("ERROR: No more events to play!");
        }

    }
}
