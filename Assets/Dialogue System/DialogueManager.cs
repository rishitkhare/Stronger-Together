using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour {
    // Animator Hashes - these allow Unity to save processing power
    // by converting the string to integer Animator Parameters
    private readonly int IsOpenHash = Animator.StringToHash("DialogueIsOpen");

    // singleton instance
    public static DialogueManager Instance;

    // Portrait sprite array
    public PortraitSpriteArray spriteArray;

    // speaker and dialogue
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portraitImage;

    // settings for dialogue
    public float dialogueSpeedCPS = 15f; //Speed in chars per second

    // timer to slow dialogue scrollrate
    private float dialogueSpeedTimer = 0f;

    // "IsOpen" is parameter controlling whether or not 
    // the dialogue box is open
    // "Speaking" parameter indicates whether still typing or not
    // --> if not typing, animate the arrow thing on the box
    private Animator animator;

    private Queue<Dialogue> dialogueQueue;

    private void Start() {
        Instance = this;
        dialogueQueue = new Queue<Dialogue>();
        animator = gameObject.GetComponent<Animator>();
    }
    //This function will be called from DialogueTrigger. It will initiate a series
    // of coroutines (methods that run across multiple frames) that each type out the text for dialogue.

    public void StartDialogue(Dialogue[] dialogue) {

        animator.SetBool(IsOpenHash, true);

        dialogueQueue.Clear();

        foreach (Dialogue dialog in dialogue) {
            dialog.Parse();
            dialogueQueue.Enqueue(dialog);
        }

        DisplayNextSentence(out bool success);
    }

    public void DisplayNextSentence(out bool success) {

        if (dialogueQueue.Count == 0) {

            CloseDialogueBox();
            success = false;
            return;

        }

        Dialogue dialog = dialogueQueue.Dequeue();

        //Set the name and portrait
        nameText.text = dialog.name;
        portraitImage.sprite = dialog.portrait;

        //type out the text using a coroutine (and skip the previous dialogue)
        string typedText = dialog.dialogText;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(typedText));
        success = true;

    }

    IEnumerator TypeSentence (string sentence) {

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            dialogueSpeedTimer = 0f;

            while(dialogueSpeedTimer < 1f / dialogueSpeedCPS) {
                dialogueSpeedTimer += Time.deltaTime;
                yield return null;
            }

        }

    }

    void CloseDialogueBox () {

        animator.SetBool(IsOpenHash, false);

    }

}
