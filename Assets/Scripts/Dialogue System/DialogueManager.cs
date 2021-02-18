using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    //speaker and dialogue
    public Text nameText;
    public Text dialogueText;
    public Sprite portraitSprite;

    // "IsOpen" is parameter controlling whether or not 
    // the dialogue box is open
    //"Speaking" parameter indicates whether still typing or not --> if not typing, animate the arrow thing on the box
    public Animator animator;

    private Queue<string> sentences;

    private void Start() {

        sentences = new Queue<string>();

    }

    public void StartDialogue(Dialogue[] dialogue) {

        foreach (Dialogue dialog in dialogue) {

            animator.SetBool("DialogueIsOpen", true);
            animator.SetBool("DialogueIsSpeaking", true);

            nameText.text = dialog.name;

            portraitSprite = dialog.portrait;

            sentences.Clear();

            foreach (string sentence in dialog.sentences)
            {

                sentences.Enqueue(sentence);

            }

            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence() {

        // starting new sentence, arrow clicker begone
        animator.SetBool("Speaking", true);

        if (sentences.Count == 0) {

            CloseDialogueBox();
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
        // I'm not sure how yield works but basically end speaking after typing is done
        EndSpeaking();

    }

    void EndSpeaking() {

        animator.SetBool("DialogueIsSpeaking", false);

    }

    void CloseDialogueBox () {

        EndSpeaking();
        animator.SetBool("DialogueIsOpen", false);

    }

}
