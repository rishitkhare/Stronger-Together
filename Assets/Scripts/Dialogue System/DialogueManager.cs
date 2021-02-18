using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    //speaker and dialogue
    public Text nameText;
    public Text dialogueText;

    // "IsOpen" is parameter controlling whether or not 
    // the dialogue box is open
    public Animator animator;

    private Queue<string> sentences;

    private void Start() {

        sentences = new Queue<string>();

    }

    public void StartDialogue(Dialogue dialogue) {

        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {

            sentences.Enqueue(sentence);

        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence() {

        if (sentences.Count == 0) {

            EndDialogue();
            return;

        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }

    IEnumerator TypeSentence (string sentence) {

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {

            dialogueText.text += letter;
            yield return null;

        }

    }

    void EndDialogue() {

        animator.SetBool("IsOpen", false);

    }

}
