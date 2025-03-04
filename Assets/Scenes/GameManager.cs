using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
//using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // Time variables
    private int day = 1;
    private int timeslot = 0;
    public List<TextMeshProUGUI> timeslotText;
    private string eventName;

    // Free time variables
    public bool isFreeTime = false;
    private bool canProgress = false;
    public List<Button> activityButtons = new List<Button>();
    public List<Button> unproductiveActivities = new List<Button>();
    public Button chatButton;
    private string selectedActivity;
    public GameObject popupWindow;
    public GameObject errorWindow;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI effectsText;
    public TextMeshProUGUI costText;
    private int activityCost;
    public GameObject resultWindow;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI resultEffects;

    // Stat variables
    private int energy = 60;
    private int stress = 50;
    private int anxiety = 50;
    private int depression = 50;
    public Slider energySlider;
    public Slider stressSlider;
    public Slider anxietySlider;
    public Slider depressionSlider;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI stressText;
    public TextMeshProUGUI anxietyText;
    public TextMeshProUGUI depressionText;
    public Image energyImage;
    public Image stressImage;
    public Image anxietyImage;
    public Image depressionImage;

    // Event system variables
    public List<GameObject> events;
    private GameObject current_event;
    private DialogueTrigger trigger;
    private DialogueManager manager;
    public GameObject dayTransition;
    public TextMeshProUGUI transitionText;
    public TextMeshProUGUI statChanges;
    public TextMeshProUGUI transitionText2;
    public GameObject timeslotPopup;
    public TextMeshProUGUI timeslotPopupText;
    public GameObject freeTimePopup;
    bool popupActive = true;
    public AudioManager audioManager;
    bool end = false;

    // Ending variables
    // Good ending = 1, bad ending = 2
    int ending = 0;
    public GameObject goodEndScreen;
    public GameObject badEndScreen;
    public GameObject gameScreen;
    // These could be replaced by lists depending on the structure of the events!
    public GameObject goodEndEvent;
    public GameObject badEndEvent;
    public GameObject endingTransScreen;
    public TextMeshProUGUI endingText;

    // For showing stat changes at the end of the day
    int startE = 60;
    int startS = 50;
    int startA = 50;
    int startD = 50;

    // Screen variables
    public GameObject roomScreen;
    public GameObject computerScreen;
    public GameObject chatScreen;    
    public GameObject current_screen;
    
    //Sprite variables
    public GameObject fullBody;
    public GameObject closeUp;
    public GameObject stressLines;
    public GameObject roomImage;
    public Sprite dayImage;
    public Sprite nightImage;



    // Start is called before the first frame update
    void Start()
    {
        // Get the dialogue manager
        manager = FindObjectOfType<DialogueManager>();

        // Get the audio manager
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void StartGame()
    {
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

        //Debug.Log("ending " + ending);

        // If there's a popup window on the screen / timeslot popup, let the user clikc to remove it
        if(popupActive)
        {
            if(Input.GetMouseButtonDown(0))
            {
                freeTimePopup.SetActive(false);
                timeslotPopup.SetActive(false);
                endingTransScreen.SetActive(false);
                popupActive = false;
            }
        }

        // Trigger the relevant screen after the ending event has finished
        if(manager.fin && end)
        {
            switch(ending)
            {
            case 0:
                break;
            case 1:
                // Good ending screen
                gameScreen.SetActive(false);
                goodEndScreen.SetActive(true);
                end = false;
                break;
            case 2:
                // Bad ending screen
                gameScreen.SetActive(false);
                badEndScreen.SetActive(true);
                end = false;
                break;
            }
        }
        
        // Dialogue codes
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

                manager.ClearChat();

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
            case 12:
                // End of this day
                if(manager.fin && manager.chatFin)
                {
                    //gameScreen.SetActive(false);
                    NextDay();
                }
                break;
            case 15:
                // Trigger the ending screen.
                if(manager.fin && manager.chatFin)
                {
                    endingTransScreen.SetActive(true);
                    //gameScreen.SetActive(false);
                    endingText.text = "Day 4";
                    popupActive = true;
                    EndGame();
                }
                break;
            case 58:
                // Toggle close up sprite
                // Rest of the functionality is in the dialogue manager
                closeUp.SetActive(true);
                break;
            case 59:
                // Toggle close up sprite
                // Rest of the functionality is in the dialogue manager
                closeUp.SetActive(false);
                break;

        }

        // If no dialogue is currenty playing, and we're not starting free time, as well as the game hasn't ended, then this is a sequence set of events in the same timeslot
        // and free time should be skipped
        if(manager.fin && manager.chatFin && manager.code != 10 && ending == 0)
        {
            Debug.Log("Skipping free time.");
            //StartCoroutine(Delay());
            NextEvent();
        }

        //Debug.Log("Dialogue ended? normal - " + manager.fin + "chat - " + manager.chatFin);
    }

    void NextEvent()
    {

        // Get the next event trigger and trigger it
        try
        {   
            //current_event = events[31];

            // Get the current event from the list and trigger it
            current_event = events[timeslot];
            trigger = current_event.GetComponent<DialogueTrigger>();
            eventName = trigger.gameObject.name;
            Debug.Log("Trigger object is: " + eventName + "timeslot: " + timeslot);

            canProgress = false;

            // Increment timeslot and text
            timeslot += 1;

            UpdateTimeslotText();

            trigger.TriggerDialogue();

            // Activate the timeslot popup if needed
            if(!manager.doesntNeedPopup)
            {
                PopupWindow(timeslotPopup, true);
            }

            // Swap image for the room
            if(eventName.Contains("Night") || eventName.Contains("Evening") || eventName.Contains("Late night"))
            {
                roomImage.GetComponent<Image>().sprite = nightImage;
                Debug.Log("Night room screen");
            }
            else
            {
                roomImage.GetComponent<Image>().sprite = dayImage;
                Debug.Log("Day room screen");
            }

        }
        // If we run out of events to play, the game is over so calculate the ending and trigger it
        catch(ArgumentOutOfRangeException)
        {
            if(manager.fin && manager.chatFin)
            {
                Debug.Log("No more events to play. Game ending now. Timeslot index: " + timeslot);
                EndGame();
            }
        }
    }

    void PopupWindow(GameObject window, bool needUpdate)
    {
        // Change the popup timeslot window text
        window.SetActive(true);
        if(needUpdate)
        {
            timeslotPopupText.text = eventName;
        }

        popupActive = true;
        //yield return new WaitForSeconds(2.5f);
    }

    void NextDay()
    {
        // Make sure we dont start another event
        // This may be redundant however
        manager.code = 0;

        // Activate the transition screen
        dayTransition.SetActive(true);

        transitionText.text = "Day " + day;

        string text = "";

        // Update the result window effect text
        text = "Energy: " + startE + " --> " + energy + "\n";
        text += "Stress: " + startS + " --> " + stress + "\n";
        text += "Anxiety: " + startA + " --> " + anxiety + "\n";
        text += "Depression: " + startD + " --> " + depression;
        statChanges.text = text;

        // Update the starting stats for tomorrow
        startE = energy;
        startS = stress;
        startA = anxiety;
        startD = depression;

        // Increment the day
        day+= 1;
    }

    public void ChangeDayNumber()
    {
        // Called from the 2nd transition screen being activated
        // Simply just changes the day number nothing else!
        transitionText2.text = "Day " + day;
    }

    public void NextDayEvents()
    {
        // Called from the 2nd transition screen being disabled
        // Simply just starts the next event in the event list

        //NextEvent();
        PopupWindow(timeslotPopup, true);
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

        PopupWindow(freeTimePopup, false);

        // Set the timeslot text
        UpdateTimeslotText();

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
        string eff = "Error: Unable to detect activity.";
        string cost = "Error: Unable to detect activity";

        // Set the text for the info window about each activity.
        switch(selectedActivity)
        {
            case "Eat":
                text = "Eat something to recover energy. Chance for a bonus effect on energy, stress, or depression.";
                eff = "Energy: " + energy + " --> " + (energy + 10);
                eff += "\n";
                eff += "Bonus +5 to energy OR -5 to stress OR depression.";
                cost = "Costs 0 Energy";
                activityCost = 0;
                break;
            case "Sleep":
                text = "Have a nap for a while to recover energy. Chance for a bonus effect on energy, stress, anxiety, or depression.";
                eff = "Energy: " + energy + " --> " + (energy + 15);
                eff += "\n";
                eff += "Bonus +5 to energy OR -5 to stress, anxiety OR depression.";
                cost = "Costs 0 Energy";
                activityCost = 0;
                break;
            case "Chat":
                text = "Chat to your friends for a bit to relieve anxiety and depression.";
                eff = "Anxiety: " + anxiety + " --> " + (anxiety - 15);
                eff += "\n";
                eff += "Depression: " + depression + " --> " + (depression - 15);
                //cost = "Energy: " + energy + " --> " + (energy - 15);
                cost = "Costs 15 Energy";
                activityCost = 15;
                break;
            case "Work":
                text = "Spend some time getting work done to relive lots of stress. Chance for a bonus effect on depression or anxiety.";
                eff = "Stress: " + stress + " --> " + (stress - 20);
                eff += "\n";
                eff += "Bonus -5 to depression OR anxiety";
                //cost = "Energy: " + energy + " --> " + (energy - 15);
                cost = "Costs 15 Energy";
                activityCost = 15;
                break;
            case "Game":
                text = "Spend time playing games to relieve stress. Chance for a bonus effect on depression or anxiety.";
                eff = "Stress: " + stress + " --> " + (stress - 15);
                eff += "\n";
                eff += "Bonus -5 to depression OR anxiety";
                //cost = "Energy: " + energy + " --> " + (energy - 10);
                cost = "Costs 10 Energy";
                activityCost = 10;
                break;
            case "Social Media":
                text = "Browse social media posts for a while to decrease depression. Chance for a bonus effect on stress or anxiety.";
                eff = "Depression: " + depression + " --> " + (depression - 15);
                eff += "\n";
                eff += "Bonus -5 to stress OR anxiety";
                //cost = "Energy: " + energy + " --> " + (energy - 10);
                cost = "Costs 10 Energy";
                activityCost = 10;
                break;
            case "Video":
                text = "Watch some videos online to decrease anxiety. Chance for a bonus effect on stress or depression.";
                eff = "Anxiety: " + anxiety + " --> " + (anxiety - 15);
                eff += "\n";
                eff += "Bonus -5 to stress OR depression";
                //cost = "Energy: " + energy + " --> " + (energy - 10);
                cost = "Costs 10 Energy";
                activityCost = 10;
                break;
            case "Music":
                text = "Listen to some music for a while to decrease anxiety and depression a bit.";
                eff = "Anxiety: " + anxiety + " --> " + (anxiety - 10);
                eff += "\n";
                eff += "Depression: " + depression + " --> " + (depression - 10);
                //cost = "Energy: " + energy + " --> " + (energy - 5);
                cost = "Costs 10 Energy";
                activityCost = 10;
                break;
        }

        descriptionText.text = text;
        effectsText.text = eff;
        costText.text = cost;

        //Debug.Log("Button name: " + buttonName);
    }

    public void SpendTime()
    {
        // If the player can't afford to do the activity, show an error.
        if(energy < activityCost)
        {
            popupWindow.SetActive(false);
            errorWindow.SetActive(true);
            return;
        }

        // Store current stats to use in the result display
        int oldEnergy = energy;
        int oldStress = stress;
        int oldAnxiety = anxiety;
        int oldDepression = depression;
        string text = "";
        string desc = "";

        // Subtract the cost of this activity
        energy -= activityCost;

        // Random number for the bonus effects
        int random1 = UnityEngine.Random.Range(0,3);
        int random2 = UnityEngine.Random.Range(0,2);
        int random3 = UnityEngine.Random.Range(0,1);

        // Affect the stats, as well as add some flavour text depending on the random stuff
        switch(selectedActivity)
        {
            case "Eat":
                energy += 15;
                desc = "You eat a meal to recover your energy. ";
                switch(random2)
                {
                    case 0:
                        energy += 5;
                        desc += "It's particularly tasty, making you feel more energised than usual.";
                        break;

                    case 1:
                        stress -= 5;
                        desc+= "You feel calm as you eat, relieving some of your stress.";
                        break;

                    case 2:
                        depression -= 5;
                        desc+= "It takes your mind off of things a little, relieving some of your depressive thoughts.";
                        break;
                }
                break;
            case "Sleep":
                energy+= 20;
                desc = "You take a nap to recover your energy. ";
                switch(random1)
                {
                    case 0:
                        energy += 5;
                        desc+= "It lasts longer than usual, leaving you extra refreshed.";
                        break;

                    case 1:
                        stress -= 5;
                        desc+= "It calmed you down, relieving some of your stress.";
                        break;

                    case 2:
                        anxiety -= 5;
                        desc+= "It helped you relax, relieving a bit of your anxieties.";
                        break;

                    case 3:
                        depression -= 5;
                        desc+= "It made you rationalise your negative depressive thoughts, relieving some of them.";
                        break;
                }
                break;
            case "Chat":
                anxiety -= 15;
                depression -= 15;
                desc = "You spend some time talking to your friends. Having so much fun with them helps you forget about your troubles for a little while, relieving some of your depressive thoughts and anxieties. ";
                break;
            case "Work":
                stress -= 20;
                desc = "You try to focus on working on your project for a while. ";
                switch(random3)
                {
                    case 0:
                        depression -= 5;
                        desc+= "You feel accomplished for your efforts, making you feel a bit better about yourself.";
                        break;

                    case 1:
                        anxiety -= 5;
                        desc+= "As you check off your to-do list, you feel less anxious.";
                        break;
                }
                break;
            case "Game":
                stress -= 15;
                desc = "You spend some time relaxing by playing one of your favourite video games. ";
                switch(random3)
                {
                    case 0:
                        depression -= 5;
                        desc+= "The game is particularly calming, relieving some of your depressive thoughts.";
                        break;

                    case 1:
                        anxiety -= 5;
                        desc+= "You feel so relaxed that you're able to forget about some of your anxieties for a little while.";
                        break;
                }
                break;
            case "Social Media":
                depression -= 15;
                desc = "You spend a while sitting in bed scrolling through social media on your phone to relieve your worries. ";
                switch(random3)
                {
                    case 0:
                        stress -= 5;
                        desc+= "The memes you saw made you laugh. You feel a bit less stressed now.";
                        break;

                    case 1:
                        anxiety -= 5;
                        desc+= "You saw cat photos. A lot of cat photos. They're so cute that they make you feel less anxious.";
                        break;
                }
                break;
            case "Video":
                anxiety -= 15;
                desc = "You spend a few hours watching videos online to relax. ";
                switch(random3)
                {
                    case 0:
                        depression -= 5;
                        desc+= "It helps you keep your mind off of your worries and troubles for a while.";
                        break;

                    case 1:
                        stress -= 5;
                        desc+= "You spend so much time laughing that it relieves some of your sress.";
                        break;
                }
                break;
            case "Music":
                anxiety -= 10;
                depression -= 10;
                desc = "You listen to some of your favourite songs for a while, calming you down. ";
                break;
        }

        // Clamp stats and change colours
        ClampStats();

        // Update the result window effect text
        if(oldEnergy != energy)
        {
            text += "Energy: " + oldEnergy + " --> " + energy + "\n";
        }
        if(oldStress != stress)
        {
            text += "Stress: " + oldStress + " --> " + stress + "\n";
        }
        if(oldAnxiety != anxiety)
        {
            text += "Anxiety: " + oldAnxiety + " --> " + anxiety + "\n";
        }
        if(oldDepression != depression)
        {
            text += "Depression: " + oldDepression + " --> " + depression;
        }

        resultEffects.text = text;
        resultText.text = desc;

        // Hide the popup and set the buttons as uninteractable now
        // Show the result popup
        popupWindow.SetActive(false);
        resultWindow.SetActive(true);
        DisableButtons();
    }

    public void FinishFreeTime()
    {
        isFreeTime = false;
        canProgress = true;

        // Free time is over, start the next event
        StartCoroutine(Delay());
        //NextEvent();
    }

    IEnumerator Delay()
    {
        // Wait for the animation to finish
        yield return new WaitForSeconds(0.5f);

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

        if(day == 2)
        {
            foreach(Button button in unproductiveActivities)
            {
                button.interactable = false;
                //Debug.Log("Set buttons false... " + button.interactable);
            }
        }

        if(day == 3)
        {
            chatButton.interactable = false;
        }
    }

    private void ClampStats()
    {
        // Clamp all the stats to between 0 and 100
        // As well as make the sliders red / white depending on their values
        // And also triggers any stat-related effects like heartbeats etc.

        audioManager.Notif();

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

        if(energy <= 25)
        {
            energyImage.color = new Color32(255, 149, 149, 255);
        }
        else
        {
            energyImage.color = new Color32(255, 255, 255, 255);
        }

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

        if(stress >= 60)
        {
            stressImage.color = new Color32(255, 149, 149, 255);
            stressLines.SetActive(true);
        }
        else
        {
            stressImage.color = new Color32(255, 255, 255, 255);
            stressLines.SetActive(false);
        }

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

        if(anxiety >= 60)
        {
            anxietyImage.color = new Color32(255, 149, 149, 255);
            Debug.Log("we should be anxious...");
            audioManager.Heartbeat();
        }
        else
        {
            anxietyImage.color = new Color32(255, 255, 255, 255);
            audioManager.StopHeartbeat();
        }

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

        if(depression >= 60)
        {
            depressionImage.color = new Color32(255, 149, 149, 255);
            audioManager.Distort();
        }
        else
        {
            depressionImage.color = new Color32(255, 255, 255, 255);
            audioManager.UnDistort();
        }
    }

    void UpdateTimeslotText()
    {
        foreach(TextMeshProUGUI t in timeslotText)
        {
            t.text = eventName;
        }
    }

    // To be called by the dialogue manager only when an event has ended.
    public void ChangeStats(int stat, int amount)
    {
        // Add / subtract the stat based on the effect amount

        switch(stat)
        {
        case 1:
            // Energy
            energy += amount;
            break;
        case 2:
            // Stress
            stress += amount;
            break;
        case 3:
            // Anxiety
            anxiety += amount;
            break;
        case 4:
            // Depression
            depression += amount;
            break;
        }

        ClampStats();

    }

    public void QuitGame()
    {
        Debug.Log("Quitting game.");
        Application.Quit();
    }

    void EndGame()
    {
        // Logic for determining which ending will go here
        // Based on adding the stats together and whether you surpass a certain "stat value" or not

        int statValue = 0;

        statValue += stress;
        statValue += anxiety;
        statValue += depression;
        statValue -= energy;

        GameObject endType;

        // Trigger the relevant ending event

        // Could use a list here if there would be multiple events in the ending. Wouldn't be hard to code.
        // Just replace the current events list with the list for the ending events here
        // And use the regular flow that the game uses to continue calling events until it runs out
        // Then use a code to trigger the ending screen rather than the next event.
        if(statValue <= 97)
        {
            //The good ending
            endType = goodEndEvent;
            ending = 1;
        }
        else
        {
            //The bad ending
            endType = badEndEvent;
            ending = 2;
        }

        // Trigger the dialogue for the relevant event
        trigger = endType.GetComponent<DialogueTrigger>();
        eventName = trigger.gameObject.name;
        trigger.TriggerDialogue();
        Debug.Log("Triggering ending sequence. Trigger object is: " + eventName);

        // Set the image for the room to day
        roomImage.GetComponent<Image>().sprite = dayImage;

        end = true;


        // Trigger the end END screen that gives further information about MI and support resources

        // Final screen to urge the player to return to the research questionnaire.

    }

    public void Restart()
    {
        // Just reloads the scene which restarts the whole game
        SceneManager.LoadScene(0);
    }
}
