using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;

public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI sentenceText;

    private Queue<string> sentences;
    private Queue<string> messages;

    public Animator animator;

    private Dialogue dialogue;
    private Dialogue chat;

    int line = 0;
    int mess = 0;

    public bool fin = false;
    public bool chatFin = true;
    public int timeslot = 0;

    public int code = 0;

    // For chat dialogues
    // Remember 1 is at the top of the screen
    public TextMeshProUGUI nameText1;
    public TextMeshProUGUI sentenceText1;
    public TextMeshProUGUI nameText2;
    public TextMeshProUGUI sentenceText2;
    public TextMeshProUGUI nameText3;
    public TextMeshProUGUI sentenceText3;
    public TextMeshProUGUI nameText4;
    public TextMeshProUGUI sentenceText4;

    // Uncover previous chat boxes
    // Again remember cover 1 is at the top
    public GameObject cover1;
    public GameObject cover2;
    public GameObject cover3;

    public Button chatNext;


    // Start is called before the first frame update
    void Awake()
    {
        // Create a Queue to hold dialogue in
        sentences = new Queue<string>();
        messages = new Queue<string>();

        nameText.text = "";
        sentenceText.text = "";

    }

    public void StartDialogue(Dialogue d)
    {
        // Set chat next button as inactive so cannot be clicked through if this dialogue happens during a chat
        chatNext.interactable = false;

        // Set global variable
        dialogue = d;

        // Reset for the Game Manager event flow
        fin = false;

        // Clear the old queue
        sentences.Clear();
        line = 0;

        Debug.Log("Starting conversation with " + dialogue.name[line]);

        /*foreach (string sentence in dialogue.sentences)
        {
            Debug.Log(sentence);
        }*/

        // Open box animation
        animator.SetBool("isOpen", true);

        // Set the speaker's name
        nameText.text = dialogue.name[0];

        // Add each sentence to the queue
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        // Delay for 1 second for the animation to play
        StartCoroutine(StartDelay());
    }

        public void DisplayNextSentence()
    {
        // End dialogue if no more sentences
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Set the speaker's name
        nameText.text = dialogue.name[line];

        // Get the code
        code = dialogue.code[line];

        // Remove the current sentence from the queue and store it
        string sentence = sentences.Dequeue();
        line += 1;

        // Set the sentence text
        //sentenceText.text = sentence;

        // Stop previous typing
        StopAllCoroutines();

        // Type letters one by one
        StartCoroutine(TypeSentence(sentence));

    }

    public void StartChat(Dialogue d)
    {
        // Set global variable
        chat = d;

        // Reset for the Game Manager event flow
        chatFin = false;

        // Only if the code isn't 5, as 5 means dont reset previous chat!
        if (chat.code[0] != 5)
        {
            Debug.Log("Resetting chats... code - " + code + " last dialogue: " + sentenceText4.text);

            // Reset old chatboxes
            nameText1.text = null;
            sentenceText1.text = null;
            nameText2.text = null;
            sentenceText2.text = null;
            nameText3.text = null;
            sentenceText3.text = null;
            nameText4.text = null;
            sentenceText4.text = null;
        }
        
        // Clear the old queue
        messages.Clear();
        mess = 0;

        Debug.Log("Starting conversation with " + chat.name[mess]);

        /*foreach (string sentence in dialogue.sentences)
        {
            Debug.Log(sentence);
        }*/

        // Open box animation
        //animator.SetBool("isOpen", true);

        // Set the speaker's name for the bottom box
        //nameText4.text = dialogue.name[0];

        // Add each sentence to the queue
        foreach (string sentence in chat.sentences)
        {
            messages.Enqueue(sentence);
        }

        // Delay for 1 second for the animation to play
        DisplayNextMessage();
    }

    public void DisplayNextMessage()
    {
        // End dialogue if no more sentences
        if (messages.Count == 0)
        {
            Debug.Log("the convo is OVER.");
            EndChat();
            return;
        }

        // Move the previous chats up
        nameText1.text = nameText2.text;
        sentenceText1.text = sentenceText2.text;

        nameText2.text = nameText3.text;
        sentenceText2.text = sentenceText3.text;

        nameText3.text = nameText4.text;
        sentenceText3.text = sentenceText4.text;

        // Set the speaker's name
        nameText4.text = chat.name[mess];

        // Get the code
        code = chat.code[mess];

        // Remove the current sentence from the queue and store it
        string sentence = messages.Dequeue();
        mess += 1;

        // Set the sentence text
        //sentenceText.text = sentence;

        UncoverBoxes();

        // Stop previous typing
        StopAllCoroutines();

        // Type letters one by one
        StartCoroutine(TypeChatSentence(sentence));

    }

    void UncoverBoxes()
    {
        /*if (nameText3.text != null)
        {
            cover3.SetActive(false);

            if(nameText2.text != null)
            {
                cover2.SetActive(false);

                if(nameText1.text != null)
                {
                    cover1.SetActive(false);
                }
            }
        }*/

        if (nameText1.text != null)
        {
            cover1.SetActive(false);
        }


        if (nameText2.text != null)
        {
            cover2.SetActive(false);
        }


        if (nameText3.text != null)
        {
            cover3.SetActive(false);
        }

    }

    IEnumerator StartDelay()
    {
        // Wait for the animation to finish
        yield return new WaitForSeconds(0.25f);

        // Start displaying a sentence
        DisplayNextSentence();
    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceText.text = "";
        
        // Type each character one by one
        foreach (char c in sentence.ToCharArray())
        {
            sentenceText.text += c;
            yield return null;
        }
    }

    IEnumerator TypeChatSentence(string sentence)
    {
        sentenceText4.text = "";
        
        // Type each character one by one
        foreach (char c in sentence.ToCharArray())
        {
            sentenceText4.text += c;
            yield return null;
        }
    }

    void EndDialogue()
    {
        // Reset chat next button.
        chatNext.interactable = true;

        Debug.Log("End of conversation.");

        // Close box animation
        animator.SetBool("isOpen", false);

        nameText.text = "";
        sentenceText.text = "";

        fin = true;
        timeslot ++;
        //Debug.Log("Fin trueeee i think - " + fin);
    }

    void EndChat()
    {
        Debug.Log("End of conversation.");

        // Close box animation
        //animator.SetBool("isOpen", false);

        chatFin = true;
        timeslot ++;
        //Debug.Log("Fin trueeee i think - " + fin);
    }
    
}
