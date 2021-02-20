using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {


    //singleton instance
    public static DialogueManager Instance;

    //Portrait sprite array
    public PortraitSpriteArray spriteArray;

    //speaker and dialogue
    public Text nameText;
    public Text dialogueText;
    public Image portraitImage;

    // "IsOpen" is parameter controlling whether or not 
    // the dialogue box is open
    //"Speaking" parameter indicates whether still typing or not --> if not typing, animate the arrow thing on the box
    public Animator animator;

    private Queue<Dialogue> dialogueQueue;

    private void Start() {
        Instance = this;
        dialogueQueue = new Queue<Dialogue>();

    }

    //This function will be called from DialogueTrigger. It will initiate a series
    // of coroutines (methods that run across multiple frames) that each type out the text for dialogue.

    public void StartDialogue(Dialogue[] dialogue) {

        foreach (Dialogue dialog in dialogue) {

            animator.SetBool("DialogueIsOpen", true);
            animator.SetBool("DialogueIsSpeaking", true);

            nameText.text = dialog.name;

            portraitImage.sprite = dialog.portrait;

            dialogueQueue.Clear();


            dialogueQueue.Enqueue(dialog);

            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence() {

        // starting new sentence, arrow clicker begone
        animator.SetBool("Speaking", true);

        if (dialogueQueue.Count == 0) {

            CloseDialogueBox();
            return;

        }

        string typedText = dialogueQueue.Dequeue().dialogText;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(typedText));

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
