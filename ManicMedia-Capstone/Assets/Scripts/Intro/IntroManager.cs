using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public Dialogue introDialogue;
    public int dialogueReached = 0;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("TutorialDialogue", 2f);
        Invoke("TutorialDialogue", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TutorialDialogue()
    {
        if(dialogueReached < 10)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(introDialogue, dialogueReached);
            dialogueReached += 1;
        }

    }

    public void DownDialogue()
    {
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "introSequencer")
        {
            TutorialDialogue();
            Destroy(other.gameObject);
        }
    }


}
