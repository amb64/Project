using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Dialogue dialogue;

    public bool isChat = false;

    public void TriggerDialogue()
    {
        if (isChat)
        {
            FindObjectOfType<DialogueManager>().StartChat(dialogue);
        }
        else
        {
            // Trigger the dialogue on the dialogue manager
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }

    }
}
