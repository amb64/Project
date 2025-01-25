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

    public Animator animator;

    private Dialogue dialogue;

    int line = 0;

    public bool fin = false;

    public int code = 0;

    // Start is called before the first frame update
    void Awake()
    {
        // Create a Queue to hold dialogue in
        sentences = new Queue<string>();

        nameText.text = "";
        sentenceText.text = "";

    }

    public void StartDialogue(Dialogue d)
    {
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

    void EndDialogue()
    {
        Debug.Log("End of conversation.");

        // Close box animation
        animator.SetBool("isOpen", false);

        nameText.text = "";
        sentenceText.text = "";

        fin = true;
        //Debug.Log("Fin trueeee i think - " + fin);
    }
    
}
