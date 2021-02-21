using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {
    // Event that freezes all entities
    // Attach functions for freezing the rest of the entities to this event in inspector.
    public UnityEvent OnDialogBegin;
    public UnityEvent OnDialogEnd;

    // Animator Hashes - these allow Unity to save processing power
    // by converting the string to integer Animator Parameters
    private readonly int IsOpenHash = Animator.StringToHash("DialogueIsOpen");

    //instance of the scene transitioner
    public SceneTransition transition;

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
    public float fasterDialogueSpeedCPS = 25f; //Speed when player actively speeds up dialogue

    // this boolean indicates that after the dialogue finishes we need to end the scene
    public bool LoadNextSceneOnDialogueEnd {get; set; }

    // information that tells us whether the box is open. This lets the
    // DialogueTrigger decide whether to initiate the next dialogue or speed up the current one.
    public bool IsTyping { get; set; }
    public bool IsFaster { get; set; }

    // private fields:
    // timer to determine dialogue scrollrate
    private float dialogueSpeedTimer = 0f;
    private Animator animator;
    private DialogueTrigger d_trigger;
    private Queue<Dialogue> dialogueQueue;

    private void Awake() {
        LoadNextSceneOnDialogueEnd = false;
        dialogueQueue = new Queue<Dialogue>();
        animator = gameObject.GetComponent<Animator>();
        d_trigger = gameObject.GetComponent<DialogueTrigger>();

        // initialize event if null to prevent nullReference shenanigans
        if (OnDialogBegin == null) { OnDialogBegin = new UnityEvent(); }
        if(OnDialogEnd == null) { OnDialogEnd = new UnityEvent(); }

        // singleton design: there should only be one DialogueManager at a time,
        // and a reference to it is stored in Instance, which can be referenced
        // from a static context (DialogueManager.Instance)
        Instance = this;
    }

    //This function will be called from DialogueTrigger. It will initiate a series
    // of coroutines (methods that run across multiple frames) that each type out the text for dialogue.
    public void StartDialogue(Dialogue[] dialogue) {
        if(dialogue != null) {
            OnDialogBegin.Invoke();

            animator.SetBool(IsOpenHash, true);

            dialogueQueue.Clear();

            foreach (Dialogue dialog in dialogue) {
                dialog.Parse();
                dialogueQueue.Enqueue(dialog);
            }

            DisplayNextSentence();
        }
        else if (LoadNextSceneOnDialogueEnd) {
            d_trigger.enabled = false;
            transition.GoToNextLevel();
        }
    }

    public void DisplayNextSentence() {

        if (dialogueQueue.Count == 0) {

            CloseDialogueBox();

            OnDialogEnd.Invoke();

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

    }

    IEnumerator TypeSentence (string sentence) {
        IsTyping = true;
        dialogueText.text = "";
        float speed = dialogueSpeedCPS;

        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            dialogueSpeedTimer = 0f;

            while(dialogueSpeedTimer < 1f / speed) {
                dialogueSpeedTimer += Time.deltaTime;

                if(IsFaster) {
                    speed = fasterDialogueSpeedCPS;
                }
                else {
                    speed = dialogueSpeedCPS;
                }

                yield return null;
            }

        }

        //reset these for the next piece of dialogue;
        IsFaster = false;
        IsTyping = false;
    }

    void CloseDialogueBox () {
        animator.SetBool(IsOpenHash, false);

        if(LoadNextSceneOnDialogueEnd) {
            d_trigger.enabled = false;
            transition.GoToNextLevel();
        }
    }

}
