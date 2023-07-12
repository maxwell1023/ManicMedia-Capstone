using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
    }
    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.L))
        {
            animator.SetBool("RadioUp", !animator.GetBool("RadioUp"));  
        }
    }

    public void StartDialogue(Dialogue dialogue, int DialoguePosition)
    {
        if (sentences.Count == 0)
        {
            animator.SetBool("RadioUp", true);

            nameText.text = dialogue.name;


            sentences.Enqueue(dialogue.sentences[DialoguePosition]);


            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("RadioUp", false);
    }

}
