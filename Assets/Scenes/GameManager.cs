using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int day = 1;
    private int timeslot = 0;

    // Free time variables
    public bool isFreeTime = false;
    private bool canProgress = false;
    public List<Button> activityButtons = new List<Button>();
    private string selectedActivity;
    public GameObject popupWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI effectsText;

    // Stat variables
    private int energy;
    private int stress;
    private int anxiety;
    private int depression;
    public Slider energySlider;
    public Slider stressSlider;
    public Slider anxietySlider;
    public Slider depressionSlider;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI stressText;
    public TextMeshProUGUI anxietyText;
    public TextMeshProUGUI depressionText;

    // Event system variables
    public List<GameObject> events;
    private GameObject current_event;
    private DialogueTrigger trigger;
    private DialogueManager manager;

    // Screen variables
    public GameObject roomScreen;
    public GameObject computerScreen;
    public GameObject chatScreen;    
    public GameObject current_screen;
    
    //Sprite variables
    public GameObject fullBody;
    public GameObject closeUp;

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
        //timeslot = manager.timeslot;
        //Debug.Log("timeslot is: " + timeslot);
        
        switch(manager.code)
        {
            case 0:
                // Do nothing
                break;
            case 1:
                NextEvent();
                break;
            case 2:
                // Swap to room screen
                current_screen.SetActive(false);
                roomScreen.SetActive(true);
                current_screen = roomScreen;

                fullBody.SetActive(false);
                closeUp.SetActive(false);
                //NextEvent();
                break;
            case 3:
                // Swap to computer screen
                current_screen.SetActive(false);
                computerScreen.SetActive(true);
                current_screen = computerScreen;

                //Debug.Log("Normally swap to computer screen.");

                fullBody.SetActive(false);
                closeUp.SetActive(false);
                //NextEvent();
                break;
            case 4:
                // Swap to chat screen
                current_screen.SetActive(false);
                chatScreen.SetActive(true);
                current_screen = chatScreen;

                fullBody.SetActive(false);
                closeUp.SetActive(false);
                //NextEvent();
                break;
            //case 5:
                // Handled in dialogue manager not here
                //break;
            case 6:
                // Toggle full body sprite
                fullBody.SetActive(true);
                break;
            case 7:
                // Toggle full body sprite
                fullBody.SetActive(false);
                break;
            case 8:
                // Toggle close up sprite
                closeUp.SetActive(true);
                break;
            case 9:
                // Toggle close up sprite
                closeUp.SetActive(false);
                break;
            case 10:
                // Free time event
                if(manager.fin && manager.chatFin && !canProgress && !isFreeTime)
                {
                    //manager.code = 0;
                    StartFreeTime();
                }
                return;
        }

        if(manager.fin && manager.chatFin && manager.code != 10)
        {
            Debug.Log("Skipping free time.");
            //StartCoroutine(Delay());
            NextEvent();
        }

        //Debug.Log("Dialogue ended? normal - " + manager.fin + "chat - " + manager.chatFin);
    }

    void NextEvent()
    {
        //Debug.Log("banana style");

        // Get the next event trigger and trigger it
        try
        {
            current_event = events[timeslot];
            trigger = current_event.GetComponent<DialogueTrigger>();
            Debug.Log("Trigger object is: " + trigger.gameObject.name);

            canProgress = false;
            timeslot += 1;
            trigger.TriggerDialogue();

        }
        catch(ArgumentOutOfRangeException)
        {
            Debug.Log("ERROR: No more events to play! Timeslot index: " + timeslot);
        }
    }

    void StartFreeTime()
    {
        // Free time timeslot
        //manager.code = 0;

        // If we currently aren't on the computer screen, move to it so we can actually have some free time
        if(current_screen != computerScreen)
        {
            current_screen.SetActive(false);
            computerScreen.SetActive(true);

            current_screen = computerScreen;

        }

        // Set the activity buttons to be interactable
        EnableButtons();

        // Disable sprites
        fullBody.SetActive(false);
        closeUp.SetActive(false);

        // Start free time
        isFreeTime = true;

        Debug.Log("Free time start!!! Yay!!");

    }

    public void FreeTimeEvent()
    {
        // Runs when an activity button is clicked

        // Get the name of the button clicked
        GameObject button = EventSystem.current.currentSelectedGameObject;
        button = button.transform.parent.gameObject;
        selectedActivity = button.name;

        // Show the popup window
        popupWindow.SetActive(true);

        // Set the title for the popup
        titleText.text = selectedActivity;
        
        // For the description text
        // If this message shows up there was some kind of error in detecting what activity was clicked.
        string text = "Error: Unable to detect activity";

        switch(selectedActivity)
        {
            case "Eat":
                text = "Eat something to recover energy. Chance for a bonus effect on energy, stress, or depression.";
                break;
            case "Sleep":
                text = "Have a nap for a while to recover energy. Chance for a bonus effect on energy, stress, anxiety, or depression.";
                break;
            case "Chat":
                text = "Chat to your friends for a bit to relieve stress. Chance for a bonus effect on depression or anxiety.";
                break;
            case "Work":
                text = "Spend some time getting work done to relive stress. Chance for a bonus effect on depression or anxiety.";
                break;
            case "Game":
                text = "Spend time playing games to relieve stress. Chance for a bonus effect on depression or anxiety.";
                break;
            case "SNS":
                text = "Browse social media posts for a while to decrease either anxiety or depression.";
                break;
            case "Video":
                text = "Watch some videos online to decrease stress and either anxiety or depression.";
                break;
            case "Music":
                text = "Listen to some music for a while to decrease anxiety and depression.";
                break;
        }

        descriptionText.text = text;

        //Debug.Log("Button name: " + buttonName);
    }

    public void SpendTime()
    {
        switch(selectedActivity)
        {
            case "Eat":
                energy += 20;
                break;
            case "Sleep":
                break;
            case "Chat":
                break;
            case "Work":
                break;
            case "Game":
                break;
            case "SNS":
                break;
            case "Video":
                break;
            case "Music":
                break;
        }

        ClampStats();

        isFreeTime = false;
        canProgress = true;

        // Hide the popup and set the buttons as uninteractable now
        popupWindow.SetActive(false);
        DisableButtons();

        // Free time is over, start the next event
        StartCoroutine(Delay());
        //NextEvent();
    }

    IEnumerator Delay()
    {
        // Wait for the animation to finish
        yield return new WaitForSeconds(1.0f);

        NextEvent();
    }

    public void DisableButtons()
    {
        foreach(Button button in activityButtons)
        {
            button.interactable = false;
            //Debug.Log("Set buttons false... " + button.interactable);
        }
    }

    public void EnableButtons()
    {
        foreach(Button button in activityButtons)
        {
            button.interactable = true;
            //Debug.Log("Set buttons true... " + button.interactable);
        }
    }

    private void ClampStats()
    {
        // Clamp all the stats to between 0 and 100

        if(energy < 0)
        {
            energy = 0;
        }
        else if(energy > 100)
        {
            energy = 100;
        }

        energySlider.value = energy;
        energyText.text = energy.ToString();

        if(stress < 0)
        {
            stress = 0;
        }
        else if(stress > 100)
        {
            stress = 100;
        }

        stressSlider.value = stress;
        stressText.text = stress.ToString();

        if(anxiety < 0)
        {
            anxiety = 0;
        }
        else if(anxiety > 100)
        {
            anxiety = 100;
        }

        anxietySlider.value = anxiety;
        anxietyText.text = anxiety.ToString();

        if(depression < 0)
        {
            depression = 0;
        }
        else if(depression > 100)
        {
            depression = 100;
        }

        depressionSlider.value = depression;
        depressionText.text = depression.ToString();
    }
}
