using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public Dialogue introDialogue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Room1Dialogue()
    {
       // if(introHappened == false)
        {
          FindObjectOfType<DialogueManager>().StartDialogue(introDialogue, 1);
         //   introHappened = true;
        }
       // if(movementLearned == false)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(introDialogue, 1);
        }

    }

    public void Room2Dialogue()
    {
       // FindObjectOfType<DialogueManager>().StartDialogue(introDialogue);
    }
    public void Room3Dialogue()
    {
       // FindObjectOfType<DialogueManager>().StartDialogue(introDialogue);
    }
    public void Room4Dialogue()
    {
       // FindObjectOfType<DialogueManager>().StartDialogue(introDialogue);
    }
}
