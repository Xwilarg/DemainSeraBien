using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;
    [Header("MolologueUI")]
    [SerializeField] private TextMeshProUGUI NPCText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Animator animator;

    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        NPCText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StartCoroutine(DisplayLine(sentence));
    }

    private void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
    
    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
